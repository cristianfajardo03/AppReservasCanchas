# reservation-booking Specification

## Purpose

Define el flujo de reserva: fecha y hora, extras opcionales, total a pagar, conflictos de horario y confirmación en memoria de la sesión.

## Requirements

### Requirement: Fecha y hora de la reserva
El sistema SHALL exigir una fecha y una franja horaria de una hora. Las franjas MUST ser de 08:00 a 21:00 inclusive (inicio). El sistema MUST rechazar fechas anteriores al día actual.

#### Scenario: Franja válida
- **WHEN** el usuario elige una fecha de hoy o futura y una hora de inicio entre 08:00 y 21:00
- **THEN** la reserva en curso queda con esa fecha y esa franja de una hora

#### Scenario: Fecha pasada
- **WHEN** el usuario intenta confirmar con una fecha anterior al día actual
- **THEN** el sistema no confirma la reserva y muestra un mensaje de error de fecha inválida

#### Scenario: Hora fuera de horario
- **WHEN** el usuario intenta usar una hora de inicio fuera del rango 08:00–21:00
- **THEN** el sistema no acepta esa franja

### Requirement: Servicios opcionales
El sistema SHALL permitir añadir o quitar, de forma independiente, iluminación, árbitro y equipamiento deportivo. Cada extra MUST tener un recargo publicado mayor que cero. Sin extras, el cargo MUST ser solo el precio base de la cancha.

#### Scenario: Reserva sin extras
- **WHEN** el usuario no marca ningún servicio opcional
- **THEN** el total mostrado es igual al precio base de la cancha seleccionada

#### Scenario: Un extra
- **WHEN** el usuario marca solo iluminación
- **THEN** el total mostrado es el precio base más el recargo de iluminación

#### Scenario: Varios extras
- **WHEN** el usuario marca iluminación, árbitro y equipamiento
- **THEN** el total mostrado es el precio base más la suma de los tres recargos

#### Scenario: Quitar un extra
- **WHEN** el usuario desmarca un extra que había marcado
- **THEN** el total deja de incluir el recargo de ese extra

### Requirement: Confirmación de reserva
El sistema SHALL confirmar una reserva solo si hay tipo de cancha, fecha y hora válidas, no hay conflicto de horario, y el cobro simulado fue exitoso. Tras confirmar, MUST guardar la reserva en memoria de la sesión y mostrar un comprobante con tipo, fecha, hora, extras, total y un identificador.

#### Scenario: Confirmación exitosa
- **WHEN** el usuario confirma con datos válidos, sin conflicto, y el cobro simulado resulta exitoso
- **THEN** el sistema guarda la reserva en memoria y muestra el comprobante con identificador, tipo, fecha, hora, extras y total

#### Scenario: Faltan datos
- **WHEN** el usuario intenta confirmar sin tipo de cancha o sin fecha y hora
- **THEN** el sistema no confirma la reserva y muestra un mensaje de datos incompletos

### Requirement: Conflicto de horario
El sistema MUST rechazar una nueva reserva si ya existe en la sesión una reserva confirmada con el mismo tipo de cancha, la misma fecha y la misma hora de inicio.

#### Scenario: Mismo tipo, fecha y hora
- **WHEN** el usuario intenta confirmar una reserva que coincide en tipo, fecha y hora de inicio con otra ya confirmada en la sesión
- **THEN** el sistema no confirma la nueva reserva y muestra un mensaje de horario no disponible

#### Scenario: Mismo horario distinto tipo
- **WHEN** ya hay una reserva de Fútbol 5 a una hora y el usuario confirma Fútbol 8 en esa misma fecha y hora
- **THEN** el sistema permite confirmar la reserva de Fútbol 8
