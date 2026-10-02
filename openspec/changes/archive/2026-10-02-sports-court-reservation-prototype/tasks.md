# Tasks

## 1. Contrato de reserva y Factory Method

- [x] 1.1 Añadir `IReservationComponent` (`GetDescription`, `GetPrice`), `CourtType` y productos `Futbol5Court`, `Futbol8Court`, `Futbol11Court` con los precios del diseño, y verificar con `dotnet build` que el proyecto compila.
- [x] 1.2 Añadir `CourtCreator` abstracto y creadores `Futbol5Creator`, `Futbol8Creator`, `Futbol11Creator` cuyo `CreateCourt()` devolva el producto correspondiente, y verificar que no existe un factory único con `switch` que haga `new` de las tres canchas.

## 2. Decorators de extras

- [x] 2.1 Implementar decorador base y `LightingDecorator`, `RefereeDecorator`, `EquipmentDecorator` (15_000, 40_000, 25_000) que sumen precio y concatenen descripción, y verificar que una cadena Fútbol 5 + los tres extras totaliza 160_000 vía `GetPrice()` (no sumando en la UI).

## 3. Adapter de pago simulado

- [x] 3.1 Implementar `SimulatedPaymentGateway.ExecuteCharge(PaymentGatewayRequest)` (AmountCents, MerchantRef, instrucción de aprobación) y `IPaymentProcessor` + `SimulatedPaymentAdapter`, y verificar que la fachada/cliente solo usa `IPaymentProcessor.Charge` y que el gateway no abre red.

## 4. Fachada y almacén en memoria

- [x] 4.1 Implementar `ReservationDraft`, `InMemoryReservationStore` y `ReservationFacade` (`GetAvailableCourtTypes`, `GetTimeSlots` 08:00–21:00, `Preview` que arma factory + decorators y rechaza fecha pasada u hora fuera de rango), y verificar con `dotnet build` y que `Preview` no instancia canchas desde fuera de los creators.
- [x] 4.2 Implementar `AuthorizePayment` (token ligado al draft; cancelación no deja token) y `Confirm` (exige token vigente, conflicto mismo tipo+fecha+hora, persistencia con `Guid`), e invalidar el token si cambia el draft; verificar los mensajes de datos incompletos, falta de pago, horario no disponible y comprobante en el contrato de la fachada.

## 5. Interfaz WinForms

- [x] 5.1 Reemplazar `Form1` por `ReservationForm` (combo de tipo, fecha, combo de hora, tres checkboxes, total y descripción desde `Preview`, lista de reservas de sesión, botones Simular pago y Confirmar reserva) y actualizar `Program.Main`; verificar con `dotnet build` que arranca el formulario nuevo.
- [x] 5.2 Enlazar cambios de controles a `Preview`, diálogo Sí/No para autorizar o cancelar cobro, y confirmación solo tras pago; verificar a mano que el total se actualiza al marcar/desmarcar extras y al cambiar de tipo de cancha.

## 6. Comprobación de integración

- [x] 6.1 Ejecutar el flujo completo: Fútbol 5 sin extras, pagar y confirmar; repetir mismo tipo/fecha/hora y ver conflicto; confirmar Fútbol 8 en el mismo horario; cancelar un cobro y comprobar que no hay reserva; intentar confirmar sin pago; verificar que el comprobante muestra identificador, tipo, fecha, hora, extras y total, y que `dotnet build` sigue en verde.
