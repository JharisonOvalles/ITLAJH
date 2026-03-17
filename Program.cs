using System;
using System.IO;
using System.Globalization;

class Program
{

    // Constantes
    const int MAX_PRODUCTOS = 100;
    const string ARCHIVO_INVENTARIO = "inventario.txt";
    const int LIMITE_STOCK_BAJO = 5;

    //Arreglos para Guardar el Inventario
    static string[] nombres = new string[MAX_PRODUCTOS];
    static decimal[] precios = new decimal[MAX_PRODUCTOS];
    static int[] existencias = new int[MAX_PRODUCTOS];
    static int totalProductos = 0;


    // Metodo principal del programa
    static void Main(string[] args)
    {
        CargarInventario();

        int opcion;

        do
        {
            MostrarMenu();
            opcion = LeerEntero("Seleccione una opción: ");

            switch (opcion)
            {
                case 1:
                    RegistrarProducto();
                    break;

                case 2:
                    ListarProductos();
                    break;

                case 3:
                    ActualizarStock();
                    break;

                case 4:
                    EliminarProducto();
                    break;

                case 5:
                    GenerarFactura();
                    break;

                case 6:
                    BuscarProducto();
                    break;

                case 0:
                    GuardarInventario();
                    Console.WriteLine("\nInventario guardado correctamente. Saliendo del sistema...");
                    break;

                default:
                    Console.WriteLine("\nOpción inválida.");
                    break;
            }

            if (opcion != 0)
            {
                Console.WriteLine("\nPresione una tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }

        } while (opcion != 0);
    }

    // Metodo que muestra el menú principal
    static void MostrarMenu()
    {
        Console.WriteLine("======================================");
        Console.WriteLine("     LA TIENDITA - INVENTARIO");
        Console.WriteLine("======================================");
        Console.WriteLine("1. Registrar producto");
        Console.WriteLine("2. Listar productos");
        Console.WriteLine("3. Actualizar stock");
        Console.WriteLine("4. Eliminar producto");
        Console.WriteLine("5. Generar factura");
        Console.WriteLine("6. Buscar producto");
        Console.WriteLine("0. Salir");
        Console.WriteLine("======================================");
    }

    // Registra un nuevo producto en el inventario.
    // Valida que el inventario no esté lleno, que el producto no exista,
    // que el precio sea mayor a 0 y que la cantidad no sea negativa.
    static void RegistrarProducto()
    {
        Console.WriteLine("\n--- REGISTRAR PRODUCTO ---");

        if (totalProductos >= MAX_PRODUCTOS)
        {
            Console.WriteLine("No se pueden registrar más productos. Inventario lleno.");
            return;
        }

        string nombre = LeerTexto("Nombre del producto: ");

        if (ExisteProducto(nombre))
        {
            Console.WriteLine("Ya existe un producto con ese nombre.");
            return;
        }

        decimal precio = LeerDecimal("Precio del producto: ");
        if (precio <= 0)
        {
            Console.WriteLine("El precio debe ser mayor que cero.");
            return;
        }

        int cantidad = LeerEntero("Cantidad inicial: ");
        if (cantidad < 0)
        {
            Console.WriteLine("La cantidad no puede ser negativa.");
            return;
        }

        nombres[totalProductos] = nombre;
        precios[totalProductos] = precio;
        existencias[totalProductos] = cantidad;
        totalProductos++;

        Console.WriteLine("Producto registrado correctamente.");
    }

    // Muestra el listado de productos registrados en el inventario
    static void ListarProductos()
    {
        Console.WriteLine("\n--- LISTADO DE PRODUCTOS ---");

        if (totalProductos == 0)
        {
            Console.WriteLine("No hay productos registrados.");
            return;
        }

        Console.WriteLine("----------------------------------------------------------------");
        Console.WriteLine("{0,-5} {1,-25} {2,-12} {3,-10} {4,-15}", "ID", "Nombre", "Precio", "Stock", "Estado");
        Console.WriteLine("----------------------------------------------------------------");

        for (int i = 0; i < totalProductos; i++)
        {
            string estado = ObtenerEstadoStock(existencias[i]);

            Console.WriteLine(
                "{0,-5} {1,-25} {2,-12} {3,-10} {4,-15}",
                i + 1,
                nombres[i],
                "RD$ " + precios[i].ToString("N2"),
                existencias[i],
                estado
            );
        }

        Console.WriteLine("----------------------------------------------------------------");
        Console.WriteLine("Total de productos: " + totalProductos);
    }

    // Permite actualizar el stock de un producto existente (agregar, restar o establecer cantidad)
    static void ActualizarStock()
    {
        Console.WriteLine("\n--- ACTUALIZAR STOCK ---");

        if (totalProductos == 0)
        {
            Console.WriteLine("No hay productos registrados.");
            return;
        }

        ListarProductos();

        int id = LeerEntero("\nIngrese el ID del producto: ") - 1;

        if (!IndiceValido(id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        Console.WriteLine("\nProducto seleccionado: " + nombres[id]);
        Console.WriteLine("Stock actual: " + existencias[id]);
        Console.WriteLine("1. Agregar stock");
        Console.WriteLine("2. Restar stock");
        Console.WriteLine("3. Reemplazar stock");

        int tipoMovimiento = LeerEntero("Seleccione una opción: ");
        int cantidad = LeerEntero("Cantidad: ");

        if (cantidad < 0)
        {
            Console.WriteLine("La cantidad no puede ser negativa.");
            return;
        }

        switch (tipoMovimiento)
        {
            case 1:
                existencias[id] += cantidad;
                Console.WriteLine("Stock actualizado correctamente.");
                break;

            case 2:
                if (cantidad > existencias[id])
                {
                    Console.WriteLine("No hay suficiente stock para realizar la salida.");
                    return;
                }

                existencias[id] -= cantidad;
                Console.WriteLine("Stock actualizado correctamente.");
                break;

            case 3:
                existencias[id] = cantidad;
                Console.WriteLine("Stock actualizado correctamente.");
                break;

            default:
                Console.WriteLine("Opción inválida.");
                break;
        }
    }

    // Elimina los productos deseados, valida que el producto este. 
    static void EliminarProducto()
    {
        Console.WriteLine("\n--- ELIMINAR PRODUCTO ---");

        if (totalProductos == 0)
        {
            Console.WriteLine("No hay productos registrados.");
            return;
        }

        ListarProductos();

        int id = LeerEntero("\nIngrese el ID del producto a eliminar: ") - 1;

        if (!IndiceValido(id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        Console.Write("Confirma eliminar el producto '" + nombres[id] + "'? (s/n): ");
        string respuesta = Console.ReadLine();

        if (respuesta == null || respuesta.Trim().ToLower() != "s")
        {
            Console.WriteLine("Operación cancelada.");
            return;
        }

        for (int i = id; i < totalProductos - 1; i++)
        {
            nombres[i] = nombres[i + 1];
            precios[i] = precios[i + 1];
            existencias[i] = existencias[i + 1];
        }

        nombres[totalProductos - 1] = "";
        precios[totalProductos - 1] = 0;
        existencias[totalProductos - 1] = 0;

        totalProductos--;

        Console.WriteLine("Producto eliminado correctamente.");
    }

    // Genera una factura de venta, descuenta el stock y guarda la factura en un archivo .txt
    static void GenerarFactura()
    {
        Console.WriteLine("\n--- GENERAR FACTURA ---");

        if (totalProductos == 0)
        {
            Console.WriteLine("No hay productos registrados.");
            return;
        }

        int[] productosFactura = new int[MAX_PRODUCTOS];
        int[] cantidadesFactura = new int[MAX_PRODUCTOS];
        int totalLineas = 0;

        while (true)
        {
            ListarProductos();

            int id = LeerEntero("\nIngrese el ID del producto (0 para terminar): ");

            if (id == 0)
            {
                break;
            }

            id--;

            if (!IndiceValido(id))
            {
                Console.WriteLine("ID inválido.");
                continue;
            }

            if (existencias[id] <= 0)
            {
                Console.WriteLine("Ese producto no tiene stock disponible.");
                continue;
            }

            int cantidad = LeerEntero("Cantidad a vender: ");

            if (cantidad <= 0)
            {
                Console.WriteLine("La cantidad debe ser mayor que cero.");
                continue;
            }

            int posicionExistente = BuscarLineaFactura(productosFactura, totalLineas, id);
            int cantidadAcumulada = cantidad;

            if (posicionExistente != -1)
            {
                cantidadAcumulada += cantidadesFactura[posicionExistente];
            }

            if (cantidadAcumulada > existencias[id])
            {
                Console.WriteLine("La cantidad solicitada supera el stock disponible.");
                continue;
            }

            if (posicionExistente == -1)
            {
                productosFactura[totalLineas] = id;
                cantidadesFactura[totalLineas] = cantidad;
                totalLineas++;
            }
            else
            {
                cantidadesFactura[posicionExistente] += cantidad;
            }

            Console.Write("Desea agregar otro producto? (s/n): ");
            string continuar = Console.ReadLine();

            if (continuar == null || continuar.Trim().ToLower() != "s")
            {
                break;
            }
        }

        if (totalLineas == 0)
        {
            Console.WriteLine("No se agregaron productos a la factura.");
            return;
        }

        string numeroFactura = DateTime.Now.ToString("yyyyMMddHHmmss");
        decimal subtotal = 0;
        decimal itbis = 0;
        decimal total = 0;

        Console.WriteLine("\n==============================================================");
        Console.WriteLine("                    FACTURA DE VENTA");
        Console.WriteLine("==============================================================");
        Console.WriteLine("Factura No.: " + numeroFactura);
        Console.WriteLine("Fecha      : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        Console.WriteLine("--------------------------------------------------------------");
        Console.WriteLine("{0,-25} {1,-8} {2,-12} {3,-12}", "Producto", "Cant.", "P.Unit", "Subtotal");
        Console.WriteLine("--------------------------------------------------------------");

        for (int i = 0; i < totalLineas; i++)
        {
            int id = productosFactura[i];
            int cantidad = cantidadesFactura[i];
            decimal subtotalLinea = precios[id] * cantidad;

            subtotal += subtotalLinea;

            Console.WriteLine(
                "{0,-25} {1,-8} {2,-12} {3,-12}",
                nombres[id],
                cantidad,
                "RD$ " + precios[id].ToString("N2"),
                "RD$ " + subtotalLinea.ToString("N2")
            );
        }

        itbis = subtotal * 0.18m;
        total = subtotal + itbis;

        Console.WriteLine("--------------------------------------------------------------");
        Console.WriteLine("Subtotal: RD$ " + subtotal.ToString("N2"));
        Console.WriteLine("ITBIS   : RD$ " + itbis.ToString("N2"));
        Console.WriteLine("Total   : RD$ " + total.ToString("N2"));
        Console.WriteLine("==============================================================");

        for (int i = 0; i < totalLineas; i++)
        {
            int id = productosFactura[i];
            existencias[id] -= cantidadesFactura[i];
        }

        GuardarFactura(numeroFactura, productosFactura, cantidadesFactura, totalLineas, subtotal, itbis, total);

        Console.WriteLine("Factura generada correctamente.");
    }

    // Busca productos en el inventario por nombre o parte del nombre
    static void BuscarProducto()
    {
        Console.WriteLine("\n--- BUSCAR PRODUCTO ---");

        if (totalProductos == 0)
        {
            Console.WriteLine("No hay productos registrados.");
            return;
        }

        string texto = LeerTexto("Ingrese nombre o parte del nombre: ").ToLower();
        bool encontrado = false;

        Console.WriteLine("----------------------------------------------------------------");
        Console.WriteLine("{0,-5} {1,-25} {2,-12} {3,-10} {4,-15}", "ID", "Nombre", "Precio", "Stock", "Estado");
        Console.WriteLine("----------------------------------------------------------------");

        for (int i = 0; i < totalProductos; i++)
        {
            if (nombres[i].ToLower().Contains(texto))
            {
                Console.WriteLine(
                    "{0,-5} {1,-25} {2,-12} {3,-10} {4,-15}",
                    i + 1,
                    nombres[i],
                    "RD$ " + precios[i].ToString("N2"),
                    existencias[i],
                    ObtenerEstadoStock(existencias[i])
                );

                encontrado = true;
            }
        }

        Console.WriteLine("----------------------------------------------------------------");

        if (!encontrado)
        {
            Console.WriteLine("No se encontraron productos.");
        }
    }

    // Guarda el inventario actual en el archivo inventario.txt
    static void GuardarInventario()
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(ARCHIVO_INVENTARIO))
            {
                sw.WriteLine(totalProductos);

                for (int i = 0; i < totalProductos; i++)
                {
                    sw.WriteLine(
                        nombres[i] + "|" +
                        precios[i].ToString(CultureInfo.InvariantCulture) + "|" +
                        existencias[i]
                    );
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al guardar el inventario: " + ex.Message);
        }
    }

    // Carga el inventario desde el archivo inventario.txt al iniciar el programa
    static void CargarInventario()
    {
        if (!File.Exists(ARCHIVO_INVENTARIO))
        {
            return;
        }

        try
        {
            using (StreamReader sr = new StreamReader(ARCHIVO_INVENTARIO))
            {
                string lineaCantidad = sr.ReadLine();

                if (string.IsNullOrWhiteSpace(lineaCantidad))
                {
                    return;
                }

                int cantidad = int.Parse(lineaCantidad);

                for (int i = 0; i < cantidad && i < MAX_PRODUCTOS; i++)
                {
                    string linea = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(linea))
                    {
                        continue;
                    }

                    string[] partes = linea.Split('|');

                    if (partes.Length != 3)
                    {
                        continue;
                    }

                    nombres[i] = partes[0];
                    precios[i] = decimal.Parse(partes[1], CultureInfo.InvariantCulture);
                    existencias[i] = int.Parse(partes[2]);
                    totalProductos++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al cargar el inventario: " + ex.Message);
        }
    }


    // Guarda la factura generada en un archivo .txt
    static void GuardarFactura(string numeroFactura, int[] productosFactura, int[] cantidadesFactura, int totalLineas, decimal subtotal, decimal itbis, decimal total)
    {
        try
        {
            string nombreArchivo = "factura_" + numeroFactura + ".txt";

            using (StreamWriter sw = new StreamWriter(nombreArchivo))
            {
                sw.WriteLine("LA TIENDITA - FACTURA DE VENTA");
                sw.WriteLine("Factura No.: " + numeroFactura);
                sw.WriteLine("Fecha      : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                sw.WriteLine("--------------------------------------------------");

                for (int i = 0; i < totalLineas; i++)
                {
                    int id = productosFactura[i];
                    int cantidad = cantidadesFactura[i];
                    decimal subtotalLinea = precios[id] * cantidad;

                    sw.WriteLine(
                        nombres[id] +
                        " | Cant: " + cantidad +
                        " | P.Unit: RD$ " + precios[id].ToString("N2") +
                        " | Subtotal: RD$ " + subtotalLinea.ToString("N2")
                    );
                }

                sw.WriteLine("--------------------------------------------------");
                sw.WriteLine("Subtotal: RD$ " + subtotal.ToString("N2"));
                sw.WriteLine("ITBIS   : RD$ " + itbis.ToString("N2"));
                sw.WriteLine("Total   : RD$ " + total.ToString("N2"));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("No se pudo guardar la factura: " + ex.Message);
        }
    }
    // Verifica si un producto ya existe en el inventario
    static bool ExisteProducto(string nombre)
    {
        for (int i = 0; i < totalProductos; i++)
        {
            if (nombres[i].Trim().ToLower() == nombre.Trim().ToLower())
            {
                return true;
            }
        }

        return false;
    }

    // Verifica si el índice corresponde a un producto válido en el inventario
    static bool IndiceValido(int indice)
    {
        return indice >= 0 && indice < totalProductos;
    }

    // Busca si un producto ya fue agregado en la factura y devuelve su posición
    static int BuscarLineaFactura(int[] productosFactura, int totalLineas, int idProducto)
    {
        for (int i = 0; i < totalLineas; i++)
        {
            if (productosFactura[i] == idProducto)
            {
                return i;
            }
        }

        return -1;
    }

    // Determina el estado del producto según la cantidad disponible en stock
    static string ObtenerEstadoStock(int cantidad)
    {
        if (cantidad == 0)
        {
            return "Agotado";
        }

        if (cantidad < LIMITE_STOCK_BAJO)
        {
            return "Stock bajo";
        }

        return "Disponible";
    }

    // Lee un texto desde consola y valida que no esté vacío
    static string LeerTexto(string mensaje)
    {
        string valor = "";

        do
        {
            Console.Write(mensaje);
            valor = Console.ReadLine();

            if (valor == null)
            {
                valor = "";
            }

            valor = valor.Trim();

            if (valor == "")
            {
                Console.WriteLine("Este campo no puede estar vacío.");
            }

        } while (valor == "");

        return valor;
    }

    // Lee un número entero desde consola y valida que sea un valor válido
    static int LeerEntero(string mensaje)
    {
        int valor;
        bool valido;

        do
        {
            Console.Write(mensaje);
            string entrada = Console.ReadLine();
            valido = int.TryParse(entrada, out valor);

            if (!valido)
            {
                Console.WriteLine("Debe ingresar un número entero válido.");
            }

        } while (!valido);

        return valor;
    }

    // Lee un número decimal desde consola y valida que sea un valor válido
    static decimal LeerDecimal(string mensaje)
    {
        decimal valor;
        bool valido;

        do
        {
            Console.Write(mensaje);
            string entrada = Console.ReadLine();

            valido = decimal.TryParse(entrada, NumberStyles.Any, CultureInfo.InvariantCulture, out valor);

            if (!valido)
            {
                valido = decimal.TryParse(entrada, out valor);
            }

            if (!valido)
            {
                Console.WriteLine("Debe ingresar un número válido.");
            }

        } while (!valido);

        return valor;
    }
}