# Spec Delta

## Purpose

Define el catálogo de tipos de cancha disponibles y cómo el usuario obtiene la cancha base de una reserva (precio y descripción según el tipo elegido).

## ADDED Requirements

### Requirement: Catálogo de tipos de cancha
El sistema SHALL ofrecer exactamente tres tipos de cancha: Fútbol 5, Fútbol 8 y Fútbol 11. Cada tipo MUST tener un nombre visible, una descripción y un precio base publicado.

#### Scenario: Usuario ve los tres tipos
- **WHEN** el usuario abre la pantalla de reserva
- **THEN** el sistema muestra Fútbol 5, Fútbol 8 y Fútbol 11 como opciones seleccionables

#### Scenario: Cada tipo tiene precio base distinto
- **WHEN** el usuario compara los tres tipos
- **THEN** cada tipo muestra un precio base mayor que cero y los tres precios base no son iguales entre sí

### Requirement: Selección de cancha base
Cuando el usuario elige un tipo de cancha, el sistema SHALL usar esa selección como cancha base de la reserva en curso y MUST mostrar el nombre, la descripción y el precio base de ese tipo.

#### Scenario: Selección de Fútbol 5
- **WHEN** el usuario selecciona Fútbol 5
- **THEN** la reserva en curso usa Fútbol 5 como cancha base y muestra su descripción y precio base

#### Scenario: Cambio de tipo
- **WHEN** el usuario cambia de Fútbol 5 a Fútbol 11
- **THEN** la reserva en curso usa Fútbol 11 como cancha base y el precio base mostrado pasa a ser el de Fútbol 11
