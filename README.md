# Sistema de Reservas de Canchas Deportivas

## Descripción

Sistema prototipo para la reserva de canchas deportivas desarrollado en C# con .NET 8 y Windows Forms.

La aplicación permite al usuario:

- Seleccionar el tipo de cancha: Fútbol 5, Fútbol 8 o Fútbol 11.
- Seleccionar una fecha y hora.
- Agregar servicios opcionales:
  - Iluminación.
  - Árbitro.
  - Equipamiento deportivo.
- Consultar el precio total de la reserva.
- Simular un pago.
- Confirmar la reserva.
- Validar conflictos de horario.

El objetivo principal del proyecto es demostrar la aplicación práctica de patrones de diseño creacionales y estructurales.

---

## Tecnologías utilizadas

- C#
- .NET 8
- Windows Forms
- Visual Studio
- OpenSpec

---

## Patrones de diseño

El proyecto implementa cuatro patrones de diseño.

### Factory Method

**Tipo:** Creacional

Se utiliza para crear los diferentes tipos de canchas sin que la interfaz tenga que instanciar directamente las clases concretas.

Los tipos de cancha disponibles son:

- Fútbol 5
- Fútbol 8
- Fútbol 11

El patrón utiliza una jerarquía de creadores para decidir qué tipo de cancha crear.

---

### Decorator

**Tipo:** Estructural

Se utiliza para agregar servicios opcionales a una reserva sin modificar las clases originales de las canchas.

Los servicios disponibles son:

- Iluminación
- Árbitro
- Equipamiento deportivo

Los servicios pueden combinarse dinámicamente.

Por ejemplo:

``text
Fútbol 5
$80.000
   ↓
+ Iluminación
$95.000
   ↓
+ Árbitro
$135.000
   ↓
+ Equipamiento
$160.000

### Adapter

Tipo: Estructural

Se utiliza para adaptar el sistema de pago simulado a la interfaz que necesita la aplicación.

La aplicación trabaja con la interfaz IPaymentProcessor, mientras que el gateway simulado utiliza una estructura diferente.

El Adapter permite que ambos componentes puedan comunicarse sin modificar el gateway.

ReservationFacade
       ↓
IPaymentProcessor
       ↓
SimulatedPaymentAdapter
       ↓
SimulatedPaymentGateway

### Facade

Tipo: Estructural

Se utiliza para simplificar el proceso completo de reserva.

La clase principal es ReservationFacade.

La fachada coordina:

Creación de la cancha mediante Factory Method.
Adición de servicios mediante Decorator.
Procesamiento del pago mediante Adapter.
Validación de los datos.
Verificación de conflictos de horario.
Confirmación de la reserva.
Almacenamiento temporal de las reservas.

De esta forma, la interfaz gráfica no necesita conocer los detalles internos de cada patrón.

### Flujo de la aplicación

El proceso principal de una reserva es:

Usuario
   ↓
Selecciona cancha
   ↓
Selecciona fecha y hora
   ↓
Agrega servicios opcionales
   ↓
ReservationFacade
   ↓
Factory Method
Crea la cancha
   ↓
Decorator
Agrega los servicios
   ↓
Calcula el precio total
   ↓
Adapter
Simula el pago
   ↓
Verifica disponibilidad
   ↓
Confirma la reserva
   ↓
Almacena la reserva en memoria

### Precios
Concepto	Precio
Fútbol 5	$80.000
Fútbol 8	$120.000
Fútbol 11	$180.000
Iluminación	+$15.000
Árbitro	+$40.000
Equipamiento	+$25.000
Ejemplo

Una reserva de Fútbol 5 con los tres servicios:

$80.000 + $15.000 + $40.000 + $25.000

Total: $160.000

### Estructura del proyecto
AppReservasCanchas/
│
├── Domain/
│   ├── CourtType.cs
│   ├── Futbol11Court.cs
│   ├── Futbol5Court.cs
│   ├── Futbol8Court.cs
│   └── IReservationComponent.cs
│
├── Patterns/
│   ├── Adapter/
│   ├── Decorator/
│   ├── Facade/
│   └── FactoryMethod/
│
├── openspec/
│
├── Program.cs
├── ReservationForm.cs
├── ReservationFlowVerifier.cs
├── README.md
└── AppReservasCanchas.csproj

### Requisitos

Para ejecutar el proyecto se necesita:

Windows.
.NET 8 SDK.
Visual Studio 2022 o superior.
Soporte para Windows Forms.

### Ejecución

Clonar el repositorio:

git clone URL_DEL_REPOSITORIO

Ingresar al proyecto:

cd AppReservasCanchas

Restaurar dependencias:

dotnet restore

Compilar:

dotnet build

Ejecutar:

dotnet run

También es posible abrir el archivo .sln directamente desde Visual Studio y ejecutar el proyecto.

### Validaciones

El sistema valida:

Selección del tipo de cancha.
Selección de fecha.
Fechas anteriores al día actual.
Horarios permitidos.
Autorización del pago antes de confirmar.
Conflictos para una misma cancha, fecha y hora.
Cambios realizados en la reserva después del pago.

Si el usuario modifica la reserva después de realizar el pago, la autorización anterior deja de ser válida.

### Pruebas realizadas

Durante el desarrollo se verificaron los siguientes escenarios:

Creación de Fútbol 5.
Creación de Fútbol 8.
Creación de Fútbol 11.
Cálculo de precios base.
Combinación de los tres servicios mediante Decorator.
Simulación de pago aprobado.
Cancelación del pago.
Intento de confirmación sin realizar el pago.
Detección de reservas duplicadas.
Reservas del mismo horario para diferentes tipos de cancha.
Confirmación y almacenamiento de reservas.

También se verificó la compilación y ejecución correcta del proyecto.

### Alcance y limitaciones

Este proyecto es un prototipo académico enfocado en demostrar el uso de patrones de diseño.

No incluye:

Base de datos.
Sistema de autenticación.
API externa.
Pasarela de pagos real.
Persistencia de información.
Sistema de usuarios.

Las reservas se almacenan únicamente en memoria durante la ejecución de la aplicación.

El sistema de pagos utilizado es completamente simulado.

### OpenSpec

El desarrollo del proyecto fue planificado y documentado utilizando OpenSpec.

La documentación correspondiente se encuentra dentro de:

openspec/

Los artefactos generados incluyen la propuesta, diseño, especificaciones y tareas de implementación.