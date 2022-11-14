using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; //Para invocar la libreria de SQL
using ABMPersonas;

namespace AppFormPersona
{
    public partial class FrmPersona : Form
    {
        bool nuevo = false;
        const int tamanio = 10; //arreglo de personas
        Persona[] aPersonas = new Persona[tamanio]; //arreglo estático de tamanio  de Personas. El tamanio define la cantidad de personas (10 en este caso). 
        int ultimo; //Para el arreglo aPersonas. 


        //Conexion me permite esablecer la conexion con la Base de datos
        SqlConnection conexion = new SqlConnection(); // se podria poner dentro del parametro
                                                      // @"Data Source=.\SQLEXPRESS;Initial Catalog=TUPPI;Integrated Security=True"

        //Comando, me permite ejecutar algun comando (DML (Insert, delete, update, select))
        SqlCommand comando = new SqlCommand();

        //No tiene constructor.
        SqlDataReader lector;


        public FrmPersona()
        {
            InitializeComponent();
        }

        private void FrmPersona_Load(object sender, EventArgs e)
        {
            habilitar(false);
            //Ese string lo saco de herramientos/conectar dato de base, buscar y copiar para pegar aca.
            //El @ sirve para que ignore los caracteres de control.
            conexion.ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=TUPPI;Integrated Security=True";
            conexion.Open();

            comando.Connection = conexion; // Utilice la conexion que acabo de abrir
            comando.CommandType = CommandType.Text; // Y que sea de tipo Texto
            comando.CommandText = " SELECT * FROM tipo_documento ";

            //Tabla es un objeto
            DataTable tabla = new DataTable();//Para almacenar estos datos que el Select
                                              //Para los comboBox es mas preferible los dataTable

            tabla.Load(comando.ExecuteReader()); //Para cargar las tablas en lo que se ejecuta en la excute reader, en este caso por que es select nos conviene reader 
                                                 //Excute Reader funciona asi = Si es text ejecutalo 
                                                 //La tabla tiene lo que se ejecuta en el select
            conexion.Close();

            cboTipoDocumento.DataSource = tabla; // se muestra en el grafico lo anterior escrito
            cboTipoDocumento.DisplayMember = "n_tipo_documento"; // Eso muestra el combo. Quiero que me muestre esa columna de la base de dato.
            cboTipoDocumento.ValueMember = "id_tipo_documento"; //El orden que tiene la BD con los numeros de id, van a aparecer en el comboBox.Eso es porque quiero que tome como valor los numero de id 

            //---------------

            // Para colocar el Estado civil (Casado, soltero, viudo y separado)
            conexion.ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=TUPPI;Integrated Security=True";
            conexion.Open();

            comando.Connection = conexion; // Utilice la conexion que acabo de abrir
            comando.CommandType = CommandType.Text; // Y que sea de tipo Texto
            comando.CommandText = " SELECT * FROM estado_civil ";

            //Tabla es un objeto
            DataTable tabla1 = new DataTable();//Para almacenar estos datos que el Select
                                               //Para los comboBox es mas preferible los dataTable

            tabla1.Load(comando.ExecuteReader()); //Para cargar las tablas en lo que se ejecuta en la excute reader, en este caso por que es select nos conviene reader 
                                                  //Excute Reader funciona asi = Si es text ejecutalo 
                                                  //La tabla tiene lo que se ejecuta en el select
            conexion.Close();

            cboEstadoCivil.DataSource = tabla1; // se muestra en el grafico lo anterior escrito
            cboEstadoCivil.DisplayMember = "n_estado_civil"; // Eso muestra el combo. Quiero que me muestre esa columna de la base de dato.
            cboEstadoCivil.ValueMember = "id_estado_civil"; //El orden que tiene la BD con los numeros de id, van a aparecer en el comboBox.Eso es porque quiero que tome como valor los numero de id 
            cboEstadoCivil.SelectedIndex = 0; // Cuando abra el formulario aparece automaticamente DNI (Posición 0 )
                                              //Si limpio se pone en todo -1.


            //   Vamos a cargar las tablas.


            this.CargarLista(LstPersonas, "personas");



        }
        //  ^  
        //Metodo CargarLista I
        private void CargarLista(ListBox lista, string nombreTabla)
        {
            ultimo = 0; //Inicializo el contador

            conexion.Open();

            comando.Connection = conexion; // Utilice la conexion que acabo de abrir
            comando.CommandType = CommandType.Text; // Y que sea de tipo Texto
            comando.CommandText = " SELECT * FROM " + nombreTabla; // <---- tabla personas, configura el comando.
            lector = comando.ExecuteReader(); // <----- Ejecuta el comando y lo deja en un lector   

            //Lea hasta no haya mas fila
            while (lector.Read()) //Se utiliza el While porque no sabemos cuantos registros hay. Si se supiera esa info hariamos un for

            {
                Persona p = new Persona();
                // sacar los datos y poner en Persona p
                if (!lector.IsDBNull(0)) ; //Si lector NO ES NULO, TRAEME EL APELLIDO
                p.pApellido = lector.GetString(0); // 0 por que es la primera columna en la BD. El getString te dice que pongas lo que recibe como apellido.

                if (!lector.IsDBNull(1)) ; //Si lector NO ES NULO, TRAEME EL NOMBRE
                p.pNombres = lector.GetString(1);

                if (!lector.IsDBNull(2)) ; //Si lector NO ES NULO, TRAEME EL TIPO DOCUMENTO
                p.pTipoDocumento = lector.GetInt32(2);
                if (!lector.IsDBNull(3)) ; //Si lector NO ES NULO, TRAEME EL DOCUMENTO
                p.pDocumento = lector.GetInt32(3);
                if (!lector.IsDBNull(4)) ; //Si lector NO ES NULO, TRAEME EL ESTADO CIVIL
                p.pEstadoCivil = lector.GetInt32(4);
                if (!lector.IsDBNull(5)) ; //Si lector NO ES NULO, TRAEME EL SEXO
                p.pSexo = lector.GetInt32(5);
                if (!lector.IsDBNull(6)) ; //Si lector NO ES NULO, TRAEME SI FALLECIO O NO.
                p.pFallecio = lector.GetBoolean(6);



                aPersonas[ultimo] = p;
                ultimo++; //Le sumo al contador


                if (ultimo == tamanio)
                {
                    //Es un mensaje de cuadro de dialogo
                    MessageBox.Show("Se completo el arreglo !!!");
                    break; //Para que se salga del WHILE
                }

            }
            lector.Close(); //Cierra el DataReader
            conexion.Close(); //Se desconecta del DataReader

            //Mostrar todos los datos en el ListBox
            lista.Items.Clear(); //Limpiame la lista para que no se acumule de mas
            for (int i = 0; i < ultimo; i++)
            {
                lista.Items.Add(aPersonas[i].ToString());
            }
            lista.SelectedIndex = 0; //Para que en la lstPersonas se para en por defecto en "PEREZ JUAN"
        }

        private void habilitar(bool x)
        {
            //Cuando se ejecuta el formulario solo aparecen disponibles los botones para 
            //utilizar (borrar, cancelar, nuevo, editar, salir y grabar).
            TxtApellido.Enabled = x;
            TxtNombre.Enabled = x;
            TxtDocumento.Enabled = x;
            cboEstadoCivil.Enabled = x;
            cboTipoDocumento.Enabled = x;
            RbtFemenino.Enabled = x;
            RbtMasculino.Enabled = x;
            chkFallecido.Enabled = x;
            BtnBorrar.Enabled = !x;
            BtnCancelar.Enabled = x;
            BtnEditar.Enabled = !x;
            BtnGrabar.Enabled = x;
            BtnNuevo.Enabled = !x;
            BtnSalir.Enabled = !x;
        }

        private void limpiar()
        {
            //Esto se utiliza para borrar todo lo que esté adentro de los casilleros.
            TxtApellido.Text = "";
            TxtNombre.Text = "";
            TxtDocumento.Text = "";
            cboEstadoCivil.SelectedIndex = -1; //Para que el comboBox no seleccione nada
            cboTipoDocumento.SelectedIndex = -1; //Para que el comboBox no seleccione nada
            RbtFemenino.Checked = false;
            RbtMasculino.Checked = false;
            chkFallecido.Checked = false;
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            nuevo = true;
            habilitar(true);
            limpiar(); //Limpia todos los casilleros
            TxtApellido.Focus(); //Focus se refiere a que se ponga en posicion de carga, es decir en Focus.
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            habilitar(true);
            TxtDocumento.Enabled = false; //Se pueden editar todos los campos menos el campo Documento. (desabilitar la PK)
            TxtApellido.Focus();
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            limpiar(); //Limpia todos los casilleros
            habilitar(false); //Desactivo y lo vuelvo en modo inicial
            nuevo = false; //Por las dudas lo pongo en falso que no este todo confirmado, para que quede todo en modo incial (false).
            CargarLista(LstPersonas, "Personas");

        }

        private void BtnGrabar_Click(object sender, EventArgs e)
        {
            //Validar los datos
            Persona p = new Persona();

            p.pNombres = TxtNombre.Text;
            p.pApellido = TxtApellido.Text;
            p.pTipoDocumento = Convert.ToInt32(TxtDocumento.Text);
            p.pEstadoCivil = Convert.ToInt32(cboEstadoCivil.SelectedValue);
            p.pFallecio = chkFallecido.Checked;

            if (RbtFemenino.Checked)
                p.pSexo = 1;
            else
                p.pSexo = 2;
            if (nuevo)
            {



                //Validar que no exista la PK !!!! 

                // insert con sentencia SQL tradicional
                string SqlInsert = "INSERT INTO Personas () VALUES ('" + p.pApellido + "','"
                                                                    + "" + p.pNombres + "', '"
                                                                    + "" + p.pEstadoCivil + "','"
                                                                    + "" + p.pFallecio + "','"
                                                                    + "" + p.pSexo + ""
                                                                    + "', '" + p.pTipoDocumento + " ')";

                //Insert usando parametros
                conexion.ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=TUPPI;Integrated Security=True";
                conexion.Open();

                comando.Connection = conexion; // Utilice la conexion que acabo de abrir
                comando.CommandType = CommandType.Text; // Y que sea de tipo Texto
                comando.CommandText = SqlInsert;
                comando.ExecuteNonQuery(); //Ejecutar sin consulta significa el ExecuteNonQuery. Ejecuta los insert, delete, upadte y los select

                conexion.Close();
            }
            else
            {
                //Update
                //Update usando parametros

                //El @ es un como un comodin antes de que reciba valores
                string updateQuery = "UPDATE Personas SET apellido=@apellido , nombres=@nombres , tipo_documento=@tipo_documento , estado_civil=@estado_civil , fallecio=@fallecio , sexo=@sexo WHERE documento=" + p.pDocumento;
                // WHERE ES DOCUMENTO PORQUE ES LA CONDICION

                insertar_o_updatear_Db(updateQuery, p);

            }
            habilitar(false);//Vuelvo todo a modo inicial
            nuevo = false; //Vuelvo todo a modo inicial
            CargarLista(LstPersonas, "Personas");

        }

        private void insertar_o_updatear_Db(string query, Persona oPersona)
        {

            conexion.Open();
            comando.Connection = conexion; // Utilice la conexion que acabo de abrir
            comando.CommandType = CommandType.Text; // Y que sea de tipo Texto


            comando.Parameters.AddWithValue("@apellido", oPersona.pApellido);
            comando.Parameters.AddWithValue("@nombres", oPersona.pNombres);
            comando.Parameters.AddWithValue("@tipo_documento", oPersona.pTipoDocumento);
            comando.Parameters.AddWithValue("@Documento", oPersona.pDocumento);
            comando.Parameters.AddWithValue("@estado_civil", oPersona.pEstadoCivil);
            comando.Parameters.AddWithValue("@sexo", oPersona.pSexo);
            comando.Parameters.AddWithValue("@fallecio", oPersona.pFallecio);

            comando.CommandText = query;
            comando.ExecuteNonQuery();//Ejecutar sin consulta significa el ExecuteNonQuery. Ejecuta los insert, delete, upadte y los select

            conexion.Close();

        }


        private void BtnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Seguro de abandonar la aplicación?",
                "SALIR", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes)

                this.Close();
        }

        private void TxtApellido_TextChanged(object sender, EventArgs e)
        {

        }

        private void LstPersonas_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarCampos(LstPersonas.SelectedIndex); //Paso las posiciones de los arreglos para que me muestre en el lstPersonas

        }

        private void cargarCampos(int posicion)
        {
            //Cargar campos desde los arreglos

            TxtApellido.Text = aPersonas[posicion].pApellido;
            TxtNombre.Text = aPersonas[posicion].pNombres;
            cboTipoDocumento.SelectedValue = aPersonas[posicion].pTipoDocumento;
            TxtDocumento.Text = Convert.ToString(aPersonas[posicion].pDocumento);
            cboEstadoCivil.SelectedValue = aPersonas[posicion].pEstadoCivil;
            //Para los radio button se utiliza IF, es uno o el otro para agregar en el arreglo. True esto o True aquello
            if (aPersonas[posicion].pSexo == 1)
                RbtFemenino.Checked = true;
            else
                RbtMasculino.Checked = true;

            chkFallecido.Checked = aPersonas[posicion].pFallecio;



        }

        private void BtnBorrar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Seguro de eliminar?",
           "BORRANDO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
           MessageBoxDefaultButton.Button2) == DialogResult.Yes)

             {
                // Delete ---> update de un campo logico

                CargarLista(LstPersonas, "Personas");
            }
        }
        
    }
}
