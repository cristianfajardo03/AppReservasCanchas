# Design

## Context

El proyecto es un WinExe WinForms `net8.0-windows` con `Form1` vacío, `Program.cs` y sin capas de dominio. Ver `proposal.md` para motivación y las specs de `court-catalog`, `reservation-booking` y `payment-simulation` para el comportamiento. El diseño debe hacer evidentes Factory Method, Decorator, Facade y Adapter en código, no solo en nombres de archivos.

## Goals / Non-Goals

**Goals:**

- Separar UI, dominio y patrones de forma que un revisor identifique cada patrón en una jerarquía clara.
- Un único punto de entrada de negocio (`ReservationFacade`) para que el formulario no orqueste fábricas, decoradores, cobro ni almacén.
- Persistencia solo en una lista en memoria de la sesión del proceso.

**Non-Goals:**

- Tests automatizados, DI container, MVVM o múltiples formularios/navegación tipo app nativa.
- Serialización a disco, usuarios o roles.
- Factory simple (un método `switch`) en lugar de Factory Method con creadores.

## Decisions

### 1. Componentes de reserva (`IReservationComponent`)

La cancha base y los extras implementan la misma interfaz observable: descripción y precio. Factory Method produce el componente concreto; Decorator lo envuelve. Alternativa descartada: un DTO plano con flags booleanos, que no demuestra Decorator.

Precios fijos (unidades de moneda locales, visibles en UI):

| Concepto        | Precio  |
|-----------------|---------|
| Fútbol 5        | 80_000  |
| Fútbol 8        | 120_000 |
| Fútbol 11       | 180_000 |
| Iluminación     | 15_000  |
| Árbitro         | 40_000  |
| Equipamiento    | 25_000  |

### 2. Factory Method para tipos de cancha

- Creador abstracto `CourtCreator` con `CreateCourt(): IReservationComponent`.
- Creadores concretos: `Futbol5Creator`, `Futbol8Creator`, `Futbol11Creator`.
- Productos: `Futbol5Court`, `Futbol8Court`, `Futbol11Court`.
- El formulario elige un `CourtType`; la fachada selecciona el creador. No hay `new` de cancha en la UI.

Alternativa descartada: un único `CourtFactory.Create(enum)` (Simple Factory), porque el enunciado pide Factory Method.

### 3. Decorator para extras

Cada extra es un decorador que suma recargo y concatena la descripción:

- `LightingDecorator`, `RefereeDecorator`, `EquipmentDecorator`.

La fachada recompone la cadena en cada vista previa (cancha nueva + decoradores según checkboxes). Cambiar tipo de cancha o extras invalida un cobro simulado previo, porque el total puede cambiar.

### 4. Adapter para cobro simulado

- Destino usado por la fachada: `IPaymentProcessor.Charge(decimal amount, string reference) -> PaymentResult`.
- Adaptee (API “ajena” local): `SimulatedPaymentGateway.ExecuteCharge(PaymentGatewayRequest)` con campos distintos (`AmountCents`, `MerchantRef`) y código de respuesta string.
- `SimulatedPaymentAdapter` traduce llamada y resultado. El gateway no usa red; `Approve()` / denegación se decide en UI (diálogo Sí/No) y se pasa al gateway como instrucción de simulación.

Alternativa descartada: que la fachada llame al gateway directamente, lo que elimina Adapter.

### 5. Facade y almacén en memoria

`ReservationFacade` expone:

- `GetAvailableCourtTypes()`, `GetTimeSlots()` (08:00–21:00).
- `Preview(ReservationDraft)` → componente decorado + total (valida fecha no pasada y hora en rango).
- `AuthorizePayment(ReservationDraft)` → preview + adapter; guarda un token de cobro en memoria ligado al hash del draft (tipo, fecha, hora, extras, total).
- `Confirm(ReservationDraft)` → exige token vigente, ausencia de conflicto (mismo tipo + fecha + hora inicio) y persiste `ConfirmedReservation` con `Guid` en `InMemoryReservationStore`.

El formulario solo enlaza controles y llama a la fachada. Alternativa descartada: lógica en event handlers de WinForms.

### 6. UI de un solo formulario

Sustituir `Form1` por `ReservationForm` (layout compacto tipo móvil: combo de tipo, fecha, combo de hora, tres checkboxes, total, lista de reservas de la sesión, botones “Simular pago” y “Confirmar reserva”). `Program.Main` instancia el nuevo formulario. Sin temas ni assets externos.

### 7. Validación de pago vs. confirmación

Dos acciones distintas para cumplir las specs de cobro cancelable y confirmación condicionada. Si el draft cambia después de autorizar, el token se descarta.

## Risks / Trade-offs

- [El revisor no ve Factory Method si se usa un `switch` único] → Mantener jerarquía `CourtCreator` / creadores concretos.
- [Decorator se “aplana” al calcular el total a mano en la UI] → El total MUST salir de `IReservationComponent.GetPrice()` de la cadena.
- [Conflicto solo en memoria] → Aceptable para prototipo; se documenta en UI (“sesión actual”).
- [Fecha “hoy” y hora ya pasada] → Las specs solo rechazan fecha anterior al día; se permite una hora ya transcurrida en el día actual (simplificación académica).

## Migration Plan

No hay datos ni usuarios. Al implementar, reemplazar el formulario vacío y ejecutar el WinExe local. Rollback: revertir el cambio en git. No hay despliegue.

## Open Questions

Ninguna que altere specs, enfoque o tareas.
