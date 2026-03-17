# 🛒 La Tiendita — Sistema de Inventario
**Proyecto Final | Curso de C# | Aplicación de Consola**

---

## 📋 Descripción

Este proyecto es un pequeño sistema de inventario desarrollado en **C# usando una aplicación de consola**.  
La idea es poder manejar los productos de una tienda sencilla: registrar artículos, controlar el stock, eliminar productos y generar facturas cuando se realiza una venta.

Todo el programa está hecho de forma **procedural**, utilizando **arreglos, métodos y estructuras básicas** como `if`, `switch`, `for` y `while`.  
No se utilizan clases ni programación orientada a objetos, ya que el objetivo del proyecto es practicar la lógica básica del lenguaje.

---

## ✅ Funcionalidades implementadas

El sistema permite realizar las siguientes operaciones:

1. **Registrar productos**  
   Se puede agregar un producto indicando nombre, precio y cantidad inicial en stock.

2. **Listar productos**  
   Muestra todos los artículos registrados con su precio y cantidad disponible.

3. **Actualizar stock**  
   Permite modificar la cantidad de un producto de tres maneras:
   - Agregar unidades (entrada)
   - Restar unidades (salida)
   - Establecer una cantidad exacta

4. **Eliminar productos**  
   Permite eliminar un producto del inventario. Antes de eliminarlo el sistema pide confirmación.

5. **Generar factura de venta**  
   Se pueden seleccionar uno o varios productos, indicar la cantidad y el sistema calcula:
   - Subtotal
   - ITBIS (18%)
   - Total a pagar

6. **Buscar producto por nombre (extra)**  
   Permite encontrar productos escribiendo el nombre completo o una parte del nombre.

7. **Guardar y cargar inventario (extra)**  
   Cuando el programa se cierra, el inventario se guarda automáticamente en un archivo llamado `inventario.txt`.  
   Cuando el sistema se vuelve a ejecutar, el inventario se carga nuevamente.

8. **Alerta de stock bajo (extra)**  
   Si un producto tiene menos de **5 unidades**, el sistema lo muestra como **stock bajo**.

9. **Guardar factura en archivo (extra)**  
   Cada factura generada también se guarda automáticamente en un archivo `.txt`.

---

## 🏗️ Estructura del proyecto

```text
LaTiendita/
├── LaTiendita.csproj
├── Program.cs
├── inventario.txt
└── factura_XXXXXX.txt
```

### Explicación

- **LaTiendita.csproj**  
  Archivo de configuración del proyecto .NET.

- **Program.cs**  
  Contiene todo el código fuente del sistema.

- **inventario.txt**  
  Archivo donde se guarda el inventario cuando el programa se cierra.

- **factura_XXXXXX.txt**  
  Archivos que se generan automáticamente cada vez que se crea una factura.

---

## 🔧 Métodos principales del programa

El código está organizado en varios métodos para separar responsabilidades.

| Método | Qué hace |
|------|------|
| `MostrarMenu()` | Muestra el menú principal del sistema |
| `RegistrarProducto()` | Agrega un nuevo producto al inventario |
| `ListarProductos()` | Muestra todos los productos registrados |
| `ActualizarStock()` | Permite aumentar, reducir o ajustar el stock |
| `EliminarProducto()` | Elimina un producto del inventario |
| `GenerarFactura()` | Permite registrar una venta y calcular el total |
| `BuscarProducto()` | Busca productos por nombre |
| `GuardarEnArchivo()` | Guarda el inventario en `inventario.txt` |
| `CargarDesdeArchivo()` | Carga el inventario al iniciar el programa |
| `LeerEnteroSeguro()` | Lee números enteros con validación |
| `LeerDoubleSeguro()` | Lee números decimales con validación |
| `VerificarStockBajo()` | Muestra alerta cuando el stock es bajo |

---

## 🚀 Cómo compilar y ejecutar

### Requisitos

Tener instalado **.NET SDK 8.0 o superior**

https://dotnet.microsoft.com/download

---

### Ejecutarlo desde la terminal

1. Entrar a la carpeta del proyecto

```bash
cd LaTiendita
```

2. Compilar el proyecto

```bash
dotnet build
```

3. Ejecutar la aplicación

```bash
dotnet run
```

---

### En Visual Studio 2022

1. Abrir el archivo **LaTiendita.csproj**
2. Presionar **F5** o el botón **Run**

---

### En VS Code

1. Abrir la carpeta **LaTiendita**
2. Instalar la extensión **C# Dev Kit**
3. Abrir la terminal y ejecutar:

```bash
dotnet run
```

---

## 🎮 Flujo básico de uso

El funcionamiento del programa es simple:

1. Registrar productos con nombre, precio y stock.
2. Listar productos para ver el inventario.
3. Generar facturas cuando se realiza una venta.
4. Al cerrar el programa, el inventario se guarda automáticamente.

Ejemplo de uso del menú:

```
1 → Registrar producto
2 → Ver inventario
5 → Generar factura
0 → Salir
```

---

## 📌 Notas técnicas

- El sistema utiliza **arreglos paralelos** para manejar el inventario:

```
nombres[]
precios[]
stock[]
```

- No se utiliza **programación orientada a objetos**, ya que el proyecto busca practicar lógica procedural.

- El sistema permite registrar **hasta 100 productos**.

- El inventario se guarda en un archivo con el siguiente formato:

```
nombre|precio|stock
```

Ejemplo:

```
Arroz|65.50|20
Aceite|120.00|10
Azucar|45.00|15
```

- Las facturas se guardan automáticamente con un nombre basado en fecha y hora.

Ejemplo:

```
factura_20260317143020.txt
```