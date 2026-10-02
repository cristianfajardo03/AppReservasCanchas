# payment-simulation Specification

## Purpose

Define el cobro simulado de una reserva: el usuario puede autorizar el pago sin pasarela, red ni datos bancarios reales, y el resultado condiciona la confirmación.

## Requirements

### Requirement: Pago simulado sin pasarela real
El sistema SHALL cobrar el total de la reserva en curso mediante una simulación local. MUST NOT conectar con pasarelas, APIs externas ni almacenar datos de tarjetas reales. El usuario MUST poder autorizar o cancelar el cobro simulado.

#### Scenario: Autorización exitosa
- **WHEN** el usuario autoriza el cobro simulado de un total mayor que cero
- **THEN** el sistema registra un cobro simulado exitoso por ese total y no envía datos a ningún servicio externo

#### Scenario: Usuario cancela el cobro
- **WHEN** el usuario cancela el cobro simulado
- **THEN** el sistema no registra cobro exitoso y no confirma la reserva

### Requirement: Confirmación condicionada al cobro
El sistema MUST confirmar la reserva solo después de un cobro simulado exitoso por el total vigente. Si el cobro no se autorizó, MUST NOT crear una reserva confirmada.

#### Scenario: Confirmar tras cobro exitoso
- **WHEN** hay un cobro simulado exitoso por el total actual y el usuario completa la confirmación
- **THEN** la reserva queda confirmada por ese total

#### Scenario: Intento de confirmar sin cobro
- **WHEN** el usuario intenta confirmar sin haber autorizado el cobro simulado
- **THEN** el sistema no crea una reserva confirmada y indica que falta el pago
