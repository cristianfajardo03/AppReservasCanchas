# Proposal

## Why

El proyecto es un prototipo académico de una app de reservas de canchas, pero hoy solo existe un formulario WinForms vacío. Hace falta un flujo de reserva demostrable (tipo de cancha, fecha y hora, extras, pago simulado y confirmación) que aplique Factory Method, Decorator, Facade y Adapter de forma explícita, sin persistencia ni integraciones reales.

## What Changes

- Reemplazar el formulario vacío por una interfaz de reserva de un solo flujo (estilo app móvil, escritorio).
- Permitir elegir tipo de cancha: Fútbol 5, Fútbol 8 o Fútbol 11, con precio base y descripción propios.
- Permitir elegir fecha y franja horaria, y componer servicios opcionales: iluminación, árbitro y equipamiento deportivo.
- Calcular el total (base + extras), simular el cobro y confirmar la reserva en memoria.
- Exponer el flujo de reserva a través de una fachada; crear canchas con Factory Method; añadir extras con Decorator; integrar el cobro simulado con Adapter.
- No añadir base de datos, autenticación, APIs externas ni pasarelas de pago reales.

## Capabilities

### New Capabilities

- `court-catalog`: catálogo de tipos de cancha (Fútbol 5, Fútbol 8, Fútbol 11) y creación de la cancha base de la reserva.
- `reservation-booking`: selección de fecha y hora, composición de extras, cálculo del total y confirmación de la reserva.
- `payment-simulation`: cobro simulado de una reserva mediante un adaptador, sin pasarela real.

### Modified Capabilities

- (ninguna; no hay specs principales aún)

## Impact

- Proyecto WinForms .NET 8 existente (`AppReservasCanchas`): `Program.cs`, `Form1` y archivos de diseño se sustituyen o reorganizan para el flujo de reserva.
- Se añaden tipos de dominio y patrones (fábricas, decoradores, fachada, adaptador) en el mismo ensamblado; persistencia en memoria.
- Sin paquetes NuGet nuevos, sin red y sin cambios de `TargetFramework`.
