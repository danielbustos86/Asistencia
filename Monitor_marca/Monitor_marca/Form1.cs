using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Net.NetworkInformation;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.IO;


namespace Monitor_marca
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public zkemkeeper.CZKEM axCZKEM1 = new zkemkeeper.CZKEM();
        string iplector="";
        private bool bIsConnected = false;//the boolean value identifies whether the device is connected
        private int iMachineNumber = 1;//the serial number of the device.After connecting the device ,this value will be changed.
        private String status = "Connect";
        int flag = 0;
        string tmpData = "";
        int Tmplength = 0;
        string id = "";
        string tipo_marca = "";
        string fecha = "";
        string Fecha_Marcacion ="";
        string Hora_marcacion="";
        string rut_trabajador = "";
        string rut_empleador = "";
        string razon_social = "";
        string direccion = "";
        string nombre_completo = "";
        string hash = "";
        string impresora = "";
        string puerto = "";
        string hora = "";


        static bool RealizarPing(string Ip)
        {
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(Ip, 3000);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch { }
            return false;
        }



        private void btnConnect_Click(object sender, EventArgs e)
        {

            //timer1.Enabled = true;
            //timer1.Start();
            
            string ip = "";

            //ip = comboBox1.<SelectedValue.ToString();
                ip=textBox1.Text;


            int idwErrorCode = 0;

            Cursor = Cursors.WaitCursor;
            if (btnConnect.Text == "DisConnect")
            {
                axCZKEM1.Disconnect();
                bIsConnected = false;
                btnConnect.Text = "Connect";
                lblState.Text = "Current State:DisConnected";
                Cursor = Cursors.Default;
                return;
            }

            bIsConnected = axCZKEM1.Connect_Net(ip, Convert.ToInt32(puerto));
            if (bIsConnected == true)
            {
                btnConnect.Text = "DisConnect";
                btnConnect.Refresh();
                lblState.Text = "Current State:Connected";
                

                iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                if (axCZKEM1.RegEvent(iMachineNumber, 65535))//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                {
                    this.axCZKEM1.OnFinger += new zkemkeeper._IZKEMEvents_OnFingerEventHandler(axCZKEM1_OnFinger);
                    this.axCZKEM1.OnVerify += new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(axCZKEM1_OnVerify);
                    this.axCZKEM1.OnAttTransactionEx += new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(axCZKEM1_OnAttTransactionEx);
                    this.axCZKEM1.OnFingerFeature += new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(axCZKEM1_OnFingerFeature);
                    this.axCZKEM1.OnEnrollFingerEx += new zkemkeeper._IZKEMEvents_OnEnrollFingerExEventHandler(axCZKEM1_OnEnrollFingerEx);
                    this.axCZKEM1.OnDeleteTemplate += new zkemkeeper._IZKEMEvents_OnDeleteTemplateEventHandler(axCZKEM1_OnDeleteTemplate);
                    this.axCZKEM1.OnNewUser += new zkemkeeper._IZKEMEvents_OnNewUserEventHandler(axCZKEM1_OnNewUser);
                    this.axCZKEM1.OnHIDNum += new zkemkeeper._IZKEMEvents_OnHIDNumEventHandler(axCZKEM1_OnHIDNum);
                    this.axCZKEM1.OnAlarm += new zkemkeeper._IZKEMEvents_OnAlarmEventHandler(axCZKEM1_OnAlarm);
                    this.axCZKEM1.OnDoor += new zkemkeeper._IZKEMEvents_OnDoorEventHandler(axCZKEM1_OnDoor);
                    this.axCZKEM1.OnWriteCard += new zkemkeeper._IZKEMEvents_OnWriteCardEventHandler(axCZKEM1_OnWriteCard);
                    this.axCZKEM1.OnEmptyCard += new zkemkeeper._IZKEMEvents_OnEmptyCardEventHandler(axCZKEM1_OnEmptyCard);
                }
            
            
            
            
            
            
            
            
            
            
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
                MessageBox.Show("Unable to connect the device,ErrorCode=" + idwErrorCode.ToString(), "Error");
                this.axCZKEM1.OnFinger -= new zkemkeeper._IZKEMEvents_OnFingerEventHandler(axCZKEM1_OnFinger);
                this.axCZKEM1.OnVerify -= new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(axCZKEM1_OnVerify);
                this.axCZKEM1.OnAttTransactionEx -= new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(axCZKEM1_OnAttTransactionEx);
                this.axCZKEM1.OnFingerFeature -= new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(axCZKEM1_OnFingerFeature);
                this.axCZKEM1.OnEnrollFingerEx -= new zkemkeeper._IZKEMEvents_OnEnrollFingerExEventHandler(axCZKEM1_OnEnrollFingerEx);
                this.axCZKEM1.OnDeleteTemplate -= new zkemkeeper._IZKEMEvents_OnDeleteTemplateEventHandler(axCZKEM1_OnDeleteTemplate);
                this.axCZKEM1.OnNewUser -= new zkemkeeper._IZKEMEvents_OnNewUserEventHandler(axCZKEM1_OnNewUser);
                this.axCZKEM1.OnHIDNum -= new zkemkeeper._IZKEMEvents_OnHIDNumEventHandler(axCZKEM1_OnHIDNum);
                this.axCZKEM1.OnAlarm -= new zkemkeeper._IZKEMEvents_OnAlarmEventHandler(axCZKEM1_OnAlarm);
                this.axCZKEM1.OnDoor -= new zkemkeeper._IZKEMEvents_OnDoorEventHandler(axCZKEM1_OnDoor);
                this.axCZKEM1.OnWriteCard -= new zkemkeeper._IZKEMEvents_OnWriteCardEventHandler(axCZKEM1_OnWriteCard);
                this.axCZKEM1.OnEmptyCard -= new zkemkeeper._IZKEMEvents_OnEmptyCardEventHandler(axCZKEM1_OnEmptyCard);
            
            }
            Cursor = Cursors.Default;

        }

        private void llenar_drop()
        {

            try
            {



                DataTable dtDatos = new DataTable();

                // Se crea un MySqlAdapter para obtener los datos de la base
                MySqlDataAdapter mdaDatos = new MySqlDataAdapter("select descripcion,ip  from con_maestro_equipos where vigente='S' and principal='S'", conexion.ObtenerCOnexion());
                



                // Con la información del adaptador se rellena el DataTable
                mdaDatos.Fill(dtDatos);

                //comboBox1.DataSource = dtDatos;
                //comboBox1.ValueMember = "ip";
                //comboBox1.DisplayMember = "descripcion";
                //comboBox1.Refresh();



            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message.ToString());
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            iplector = ConfigurationManager.AppSettings["iplector"];
            puerto = ConfigurationManager.AppSettings["puerto"];
            impresora = ConfigurationManager.AppSettings["impresora"];

            
            textBox1.Text = iplector;
            llenar_drop();
        
            //timer1.Start();

        }


        public void limpiar_list() {        
            if (ListaEstado.Items.Count>10){

                ListaEstado.Items.Clear();

            } 
        }
        
        private void axCZKEM1_OnAttTransactionEx(string sEnrollNumber, int iIsInValid, int iAttState, int iVerifyMethod, int iYear, int iMonth, int iDay, int iHour, int iMinute, int iSecond, int iWorkCode)
        {
            limpiar_list();

            try { 

            ListaEstado.Items.Add("Verificación OK.");
            ListaEstado.Items.Add("ID Usuario:" + sEnrollNumber);

            ListaEstado.Items.Add("Metodo de Verificación:" + iVerifyMethod.ToString());
            ListaEstado.Items.Add("Fecha Hora:" + iDay.ToString() + "-" + iMonth.ToString() + "-" + iYear.ToString() + " " + iHour.ToString() + ":" + iMinute.ToString() + ":" + iSecond.ToString());
            


            string fecha = iYear.ToString() + "-" + iMonth.ToString() + "-" + iDay.ToString() + " " + iHour.ToString() + ":" + iMinute.ToString() + ":" + iSecond.ToString();

                    DateTime myDate = DateTime.Parse(fecha);
                    DataTable dtDatos = new DataTable();


                    String query = @"INSERT INTO marcas(idpersona,fecha_marcaje,ip,movimiento,metodo,hasht) VALUES(?idpersona,?fecha_marcaje,?ip,?movimiento,?metodo,?hasht)";
                    MySqlCommand cmd = new MySqlCommand();


// _idpersona 	int,
//_fecha_registro datetime,
//_ip 			varchar(20),
//_movimiento 	char(1),
//_metodo 		int, 
//_hasht 			varchar(100))

                    string mov = iAttState.ToString();
                    string hash1 = iYear.ToString() + "-" + iMonth.ToString() + "-" + iDay.ToString() + " " + iHour.ToString() + ":" + iMinute.ToString()+":"+iSecond.ToString();
                    string ip = textBox1.Text;
                    string hash3 = hash1+ip;
                    
                    string hashfinal = GetEncodedHash(hash3, "123");
 
   
                     DataTable dtDatosres = new DataTable();

                     dtDatosres = Inserta_registro(sEnrollNumber, myDate, mov, ip, mov, iVerifyMethod.ToString(), hashfinal);


                     if (dtDatosres.Rows.Count > 0)
                    {

                        DataRow row = dtDatosres.Rows[0];
                        //tipo_marca = row["TIPO_MARCA"].ToString();
                        id = sEnrollNumber;
                        string mostrar_mov = "";
                         if (mov == "0") 
                         {
                             mostrar_mov ="ENTRADA";
                         }
                          if (mov == "1") 
                         {
                             mostrar_mov ="SALIDA";
                         }

                          if (mov == "2") 
                         {
                             mostrar_mov ="COMIENZA COLACION";
                         }

                          if (mov == "4") 
                         {
                             mostrar_mov ="REGRESO COLACION";
                         }

                         rut_trabajador = row["rut_t"].ToString();
                         nombre_completo = row["nombre_completo"].ToString(); 
                         tipo_marca = mostrar_mov;
                         Fecha_Marcacion = iDay.ToString() + "/" + iMonth.ToString() + '/' + iYear.ToString();

                         int hor = 0;
                         int min = 0;
                         int seg = 0;
                         hor = iHour;
                         min = iMinute;
                         seg = iSecond;
                         string shora = "";
                         string smin = "";
                         string sseg = "";

                         if (iHour < 10) {

                             shora = "0" + iHour.ToString();

                         }
                         else
                         {
                             shora = iHour.ToString();

                         }


                         if (iMinute < 10)
                         {

                             smin = "0" + iMinute.ToString();

                         }
                         else
                         {
                             smin = iMinute.ToString();


                         }
                         if (iSecond < 10)
                         {

                             sseg = "0" + iSecond.ToString();

                         }
                         else
                         {
                             sseg = iSecond.ToString();


                         }


                         Hora_marcacion = shora + ":" + smin + ":" + sseg;
                      
                         rut_empleador  = row["rut_e"].ToString();
                         razon_social   = row["razonsocial"].ToString();
                         direccion      = row["direccion"].ToString();
                         hash = hashfinal;
                       


                        ListaEstado.Items.Add("Verificacion registrada en Base de datos");
                        id = sEnrollNumber;

                        
                        if (cbx_imprime.Checked == true)
                        {
                            imprimir();

                        
                       }

                        if (row["acepta_mail"].ToString() == "S")
                        {
                            string mailt = "";
                            int idmarca = 0;
                            mailt = row["mail"].ToString();
                            idmarca = Convert.ToInt32(row["ultima_marca"].ToString());
                            string asuntom = "COMPROBANTE " + mostrar_mov + " " + Fecha_Marcacion;



                            string cuerpo = "";

                            cuerpo = "<table border=1 cellpadding=0 cellspacing=0>";
                            cuerpo = cuerpo + "  <tr><td><table border=0 cellpadding=0 cellspacing=0><tr>";
                            cuerpo = cuerpo + "<td width=90 height=40>&nbsp;</td>";
                            cuerpo = cuerpo + "<td width=257 align=center valign=middle><strong>COMPROBANTE "+ mostrar_mov  +"</strong></td>";
                            cuerpo = cuerpo + "<td width=99>&nbsp;</td></tr><tr><td colspan=3><table width=448 border=0>";
                            cuerpo = cuerpo + "<tr> <td width=218 align=center>" + rut_trabajador + "</td><td width=206 align=center>" + Fecha_Marcacion + " " + Hora_marcacion + "</td>";
                            cuerpo = cuerpo + "</tr></table></td></tr><tr><td>&nbsp;</td><td><div align=center>" + razon_social + "</div></td><td>&nbsp;</td>";
                            cuerpo = cuerpo + "</tr><tr><td>&nbsp;</td><td><div align=center>" + rut_empleador + "</div></td><td>&nbsp;</td></tr><tr>";
                            cuerpo = cuerpo + "<td colspan=3><div align=center>" + direccion + "</div></td></tr><tr>";
                            cuerpo = cuerpo + "<td colspan=3 align=center><h6>ID: " + hash + "</h6></td></tr></table></td></tr></table>";


                            string cuerpom = cuerpo; 
                            int respuesta = 0;
                            respuesta = Inserta_colamail(idmarca, asuntom, mailt, cuerpom);




                        }


                    };


            //                   ListaEstado.Items.Add("Verificacion registrada en Base de datos");
                   

                   //}

            }
            catch (Exception e)
            {
                ListaEstado.Items.Add("error en evento monitoreo");
            }
         }



        private void axCZKEM1_OnFingerFeature(int iScore)
        {
            if (iScore < 0)
            {
                 limpiar_list();
                //System.Console.WriteLine("The quality of your fingerprint is poor");
                ListaEstado.Items.Add("La calidad de su huella digital es baja.");
                ListaEstado.TopIndex = ListaEstado.Items.Count - 1;

            }
            else
            {
                //System.Console.WriteLine("RTEvent OnFingerFeature Has been Triggered...Score:　" + iScore.ToString());
                limpiar_list();
                ListaEstado.Items.Add("RTEvent OnFingerFeature Has been Triggered...Score:　" + iScore.ToString());
                ListaEstado.TopIndex = ListaEstado.Items.Count - 1;

            }
        }

        private void axCZKEM1_OnFinger()
        {
            //System.Console.WriteLine("RTEvent OnFinger Has been Triggered");
            limpiar_list();
            ListaEstado.Items.Add("RTEvent OnFinger Has been Triggered");
            ListaEstado.TopIndex = ListaEstado.Items.Count - 1;

        }

        private void axCZKEM1_OnVerify(int iUserID)
        {
            //System.Console.WriteLine("RTEvent OnVerify Has been Triggered,Verifying...");
            limpiar_list();
            ListaEstado.Items.Add("El evento de validación se ha disparado.");
            ListaEstado.TopIndex = ListaEstado.Items.Count - 1;

            if (iUserID != -1)
            {
                //System.Console.WriteLine("Verified OK,the UserID is " + iUserID.ToString());
                ListaEstado.Items.Add("Verified OK,the UserID is ---1 " + iUserID.ToString());
                ListaEstado.TopIndex = ListaEstado.Items.Count - 1;
            }
            else
            {
                //System.Console.WriteLine("Verified Failed... ");

                ListaEstado.Items.Add("Validación Fallida.");
                ListaEstado.TopIndex = ListaEstado.Items.Count - 1;
                DateTime fecha=DateTime.Now;

                if (Inserta_error(fecha, textBox1.Text) == 1)
                {

                    ListaEstado.Items.Add("Se registra Marca fallida en BD");
                    ListaEstado.TopIndex = ListaEstado.Items.Count - 1;
                    limpiar_list();
                    if (cbx_imprime.Checked)
                    {
                        
                        imprimirFalla();   
                    }

                }

            }
        }

        //When you are enrolling your finger,this event will be triggered.
        private void axCZKEM1_OnEnrollFingerEx(string sEnrollNumber, int iFingerIndex, int iActionResult, int iTemplateLength)
        {
            if (iActionResult == 0)
            {
                //System.Console.WriteLine("RTEvent OnEnrollFigerEx Has been Triggered....");
                //System.Console.WriteLine(".....UserID: " + sEnrollNumber + " Index: " + iFingerIndex.ToString() + " tmpLen: " + iTemplateLength.ToString());
                limpiar_list();
                ListaEstado.Items.Add("RTEvent OnEnrollFigerEx Has been Triggered....");
                ListaEstado.Items.Add(".....UserID: " + sEnrollNumber + " Index: " + iFingerIndex.ToString() + " tmpLen: " + iTemplateLength.ToString());
                ListaEstado.TopIndex = ListaEstado.Items.Count - 1;
            }
            else
            {
                //System.Console.WriteLine("RTEvent OnEnrollFigerEx Has been Triggered Error,actionResult=" + iActionResult.ToString());

                ListaEstado.Items.Add("RTEvent OnEnrollFigerEx Has been Triggered Error,actionResult=" + iActionResult.ToString());
                ListaEstado.TopIndex = ListaEstado.Items.Count - 1;
            }
        }

        //When you have deleted one one fingerprint template,this event will be triggered.
        private void axCZKEM1_OnDeleteTemplate(int iEnrollNumber, int iFingerIndex)
        {
            limpiar_list();
            ListaEstado.Items.Add("RTEvent OnDeleteTemplate Has been Triggered...");
            ListaEstado.Items.Add("...UserID= 3" + iEnrollNumber.ToString() + " FingerIndex=" + iFingerIndex.ToString());

            ListaEstado.TopIndex = ListaEstado.Items.Count - 1;
        }

        //When you have enrolled a new user,this event will be triggered.
        private void axCZKEM1_OnNewUser(int iEnrollNumber)
        {
            limpiar_list();
            ListaEstado.Items.Add("RTEvent OnNewUser Has been Triggered...");
            ListaEstado.Items.Add("...NewUserID= " + iEnrollNumber.ToString());
            ListaEstado.TopIndex = ListaEstado.Items.Count - 1;

        }

        //When you swipe a card to the device, this event will be triggered to show you the card number.
        private void axCZKEM1_OnHIDNum(int iCardNumber)
        {
            limpiar_list();
            ListaEstado.Items.Add("RTEvent OnHIDNum Has been Triggered...");
            ListaEstado.Items.Add("...Cardnumber= 5" + iCardNumber.ToString());

            ListaEstado.TopIndex = ListaEstado.Items.Count - 1;
        }

        //When the dismantling machine or duress alarm occurs, trigger this event.
        private void axCZKEM1_OnAlarm(int iAlarmType, int iEnrollNumber, int iVerified)
        {
            limpiar_list();
            ListaEstado.Items.Add("RTEvnet OnAlarm Has been Triggered...");
            ListaEstado.Items.Add("...AlarmType=" + iAlarmType.ToString());
            ListaEstado.Items.Add("...EnrollNumber=" + iEnrollNumber.ToString());
            ListaEstado.Items.Add("...Verified=" + iVerified.ToString());

            ListaEstado.TopIndex = ListaEstado.Items.Count - 1;

        }

        //Door sensor event
        private void axCZKEM1_OnDoor(int iEventType)
        {
            limpiar_list();
            ListaEstado.Items.Add("RTEvent Ondoor Has been Triggered...");
            ListaEstado.Items.Add("...EventType=" + iEventType.ToString());

            ListaEstado.TopIndex = ListaEstado.Items.Count - 1;
        }

        //When you have written into the Mifare card ,this event will be triggered.
        private void axCZKEM1_OnWriteCard(int iEnrollNumber, int iActionResult, int iLength)
        {
            ListaEstado.Items.Add("RTEvent OnWriteCard Has been Triggered...");

            if (iActionResult == 0)
            {
                ListaEstado.Items.Add("...Write Mifare Card OK");
                ListaEstado.Items.Add("...EnrollNumber=" + iEnrollNumber.ToString());
                ListaEstado.Items.Add("...TmpLength=" + iLength.ToString());

                ListaEstado.TopIndex = ListaEstado.Items.Count - 1;

            }
            else
            {
                ListaEstado.Items.Add("...Write Failed");

                ListaEstado.TopIndex = ListaEstado.Items.Count - 1;
            }
        }

        //When you have emptyed the Mifare card,this event will be triggered.
        private void axCZKEM1_OnEmptyCard(int iActionResult)
        {
            ListaEstado.Items.Add("RTEvent OnEmptyCard Has been Triggered...");
            if (iActionResult == 0)
            {

                ListaEstado.Items.Add("...Empty Mifare Card OK");
            }
            else
            {

                ListaEstado.Items.Add("...Empty Failed");
            }
            ListaEstado.TopIndex = ListaEstado.Items.Count - 1;
        }


         //   string Hora = DateTime.Now.ToString();
            //  validaHora();

            //ListaEstado.Items.Add("...Invalido:" + iIsInValid.ToString());
            //ListaEstado.Items.Add("...Estado:" + iAttState.ToString());
            //ListaEstado.Items.Add("...Codigo Trabajo:" + iWorkCode.ToString());
            //ListaEstado.Items.Add("...Fecha Hora:" + iYear.ToString() + "-" + iMonth.ToString() + "-" + iDay.ToString() + " " + iHour.ToString() + ":" + iMinute.ToString() + ":" + iSecond.ToString());


            


            //ListaEstado.TopIndex = ListaEstado.Items.Count - 1;

            //int numerolineas = 0;
            //numerolineas = ListaEstado.Items.Count;

            //if (numerolineas > 40);
            //{
            //    DateTime dt = DateTime.Now;
            //    string archivonom = dt.ToString("dd-MM-yyyy-HHmmss");
            //    string appPath = Directory.GetCurrentDirectory();
            //    appPath = appPath + "\\Temp\\" + archivonom + ".txt";
            //    System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(appPath);
            //    foreach (var item in ListaEstado.Items)
            //    {
            //        SaveFile.WriteLine(item);
            //    }
            //    SaveFile.Close();         
            //}





        public static DataTable Inserta_registro(string id_persona, DateTime _fecha_marcaje, string tipo_marca, string ip, string evento, string metodo,string _hash)
        {
    
            MySqlCommand cmd = new MySqlCommand();

         //   DataTable dtDatos = new DataTable();
            // Se crea un MySqlAdapter para obtener los datos de la base
            // MySqlDataAdapter mdaDatos = new MySqlDataAdapter("select id_persona  from con_huella where ID_PERSONA =?ID_PERSONA", conexion.ObtenerCOnexion());

            MySqlDataAdapter mdaDatos = new MySqlDataAdapter();
            using (DataTable dtDatos = new DataTable())
            { 
            try
            {


                cmd.Connection = conexion.ObtenerCOnexion();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "INSERTA_MARCA";

                cmd.Parameters.AddWithValue("_idpersona", id_persona);
                cmd.Parameters.AddWithValue("_fecha_marcaje", _fecha_marcaje);
                cmd.Parameters.AddWithValue("_ip", ip);
                cmd.Parameters.AddWithValue("_movimiento", evento);
                cmd.Parameters.AddWithValue("_metodo", metodo);

                cmd.Parameters.AddWithValue("_hasht", _hash);

                
                mdaDatos.SelectCommand = cmd;
                mdaDatos.Fill(dtDatos);


            }
            catch (MySqlException mysqlex)
            {
                MessageBox.Show(mysqlex.Message.ToString());
                
                
            }

            
            return dtDatos;
            }
        }


        public int Inserta_error(DateTime _fecha, string _ip)
        {

            MySqlCommand cmd = new MySqlCommand();
            int retorno = 0;
            //   DataTable dtDatos = new DataTable();
            // Se crea un MySqlAdapter para obtener los datos de la base
            // MySqlDataAdapter mdaDatos = new MySqlDataAdapter("select id_persona  from con_huella where ID_PERSONA =?ID_PERSONA", conexion.ObtenerCOnexion());
            string aux = _fecha.ToString() + _ip;

            string HashFinal = "";
            HashFinal= GetEncodedHash(aux, "123");
            hash = HashFinal;
            Fecha_Marcacion = _fecha.ToString();

            
                try
                {


                    cmd.Connection = conexion.ObtenerCOnexion();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Inserta_marcas_fallidas";

                    cmd.Parameters.AddWithValue("_ip", _ip);
                    cmd.Parameters.AddWithValue("_fecha", _fecha);

                    cmd.Parameters.AddWithValue("_hasht", HashFinal);
                    cmd.ExecuteNonQuery();

                    retorno = 1;

                }
                catch (MySqlException mysqlex)
                {
                    
                    ListaEstado.Items.Add(mysqlex.Message.ToString());

                    retorno = 0;
                }
                finally
                {

                    cmd.Connection.Close();
                }


                return retorno;
        }



        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
//            rut_trabajador;
//nombre_completo  



            
            // Create font and brush.
            Font drawFont = new Font("Arial", 6);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            // Create point for upper-left corner of drawing.
            
            // Draw string to screen.
            e.Graphics.DrawString("RUT TRA:" + rut_trabajador, drawFont, drawBrush, 10, 20);

           // e.Graphics.DrawString(rut_trabajador, drawFont, drawBrush, 80, 20);

            e.Graphics.DrawString("NOMBRE:" + nombre_completo, drawFont, drawBrush, 10, 40);
         //   e.Graphics.DrawString(nombre_completo, drawFont, drawBrush, 80, 40);

            e.Graphics.DrawString("EVENTO:" + tipo_marca, drawFont, drawBrush, 10, 60);
//            e.Graphics.DrawString(tipo_marca, drawFont, drawBrush, 80, 60);

            e.Graphics.DrawString("FECHA:" + Fecha_Marcacion, drawFont, drawBrush, 10, 80);
           // e.Graphics.DrawString(Fecha_Marcacion, drawFont, drawBrush, 80, 80);

            e.Graphics.DrawString("HORA:" + Hora_marcacion, drawFont, drawBrush, 10, 100);
           // e.Graphics.DrawString(Hora_marcacion, drawFont, drawBrush, 80, 100);

            e.Graphics.DrawString("RUT:" + rut_empleador, drawFont, drawBrush, 10, 120);
            //e.Graphics.DrawString(rut_empleador, drawFont, drawBrush, 80, 120);

            e.Graphics.DrawString("EMPRESA:" + razon_social, drawFont, drawBrush, 10, 140);
            //e.Graphics.DrawString(razon_social, drawFont, drawBrush, 80, 140);

            e.Graphics.DrawString("DIRECCION:" + direccion, drawFont, drawBrush, 10, 160);
            //e.Graphics.DrawString(direccion, drawFont, drawBrush, 80, 160);


            e.Graphics.DrawString("HASH:" + hash, drawFont, drawBrush, 10, 180);
            //e.Graphics.DrawString(hash, drawFont, drawBrush, 80, 180);

            e.HasMorePages = false;

            //rut_empleador
            //razon_social 
            //direccion                            



        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    printDocument1.PrinterSettings.PrinterName = "XP-80C";
        //    printDocument1.PrintController = new System.Drawing.Printing.StandardPrintController();


        //    printDocument1.Print();

        //}
            
       private  void  imprimir() {

           printDocument1.PrinterSettings.PrinterName = impresora;
           printDocument1.PrintController = new System.Drawing.Printing.StandardPrintController();


           printDocument1.Print();

           
       }
       private void imprimirFalla()
       {

           printDocument2.PrinterSettings.PrinterName = impresora;
           printDocument2.PrintController = new System.Drawing.Printing.StandardPrintController();


           printDocument2.Print();


       }
       private void timer1_Tick(object sender, EventArgs e)
       {
           if (RealizarPing(iplector) == false)
           {
               ListaEstado.Items.Add("Sin conexion");
               


           } 


       }

       private void button2_Click(object sender, EventArgs e)
       {
           
            string sdwEnrollNumber = "";
            string sName = "";
            int idwFingerIndex = 0;
            string sTmpData = "";
            int iPrivilege = 0;
            string sPassword = "";
            string sEnabled = "";
            bool bEnabled = false;
            int iFlag = 1;

            int iUpdateFlag = 1;

               axCZKEM1.EnableDevice(iMachineNumber, false);
            if (axCZKEM1.BeginBatchUpdate(iMachineNumber, iUpdateFlag))//create memory space for batching data
            {
                sdwEnrollNumber="9999";
                sName ="ADMINISTRADOR";
                sPassword="1234";
                iPrivilege =1;
                bEnabled = true;



            if (axCZKEM1.SSR_SetUserInfo(iMachineNumber, sdwEnrollNumber, sName, sPassword, iPrivilege, bEnabled)){
                axCZKEM1.BatchUpdate(iMachineNumber);//upload all the information in the memory
                axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
                Cursor = Cursors.Default;
                axCZKEM1.EnableDevice(iMachineNumber, true);
                MessageBox.Show("Revisa caeza de chancho");
            }

            }
       }

       private void button3_Click(object sender, EventArgs e)
       {
           int idwErrorCode = 0;

           Cursor = Cursors.WaitCursor;

           if (btnUSBConnect.Text == "Disconnect")
           {
               axCZKEM1.Disconnect();
               bIsConnected = false;
               btnUSBConnect.Text = "Connect";
               btnUSBConnect.Refresh();
               lblState.Text = "Current State:Disconnected";
               Cursor = Cursors.Default;
               return;
           }

           if (rbUSB.Checked == true)//the common USBClient
           {
               iMachineNumber = 1;//In fact,when you are using common USBClient communication,parameter Machinenumber will be ignored,that is any integer will all right.Here we use 1.
               bIsConnected = axCZKEM1.Connect_USB(iMachineNumber);
           }
           //else
           //    if (rbVUSB.Checked == true)//connect the device via the virtual serial port created by USB
           //    {
           //        SearchforUSBCom usbcom = new SearchforUSBCom();
           //        string sCom = "";
           //        bool bSearch = usbcom.SearchforCom(ref sCom);//modify by Darcy on Nov.26 2009
           //        if (bSearch == false)//modify by Darcy on Nov.26 2009
           //        {
           //            MessageBox.Show("Can not find the virtual serial port that can be used", "Error");
           //            Cursor = Cursors.Default;
           //            return;
           //        }

           //        int iPort;
           //        for (iPort = 1; iPort < 10; iPort++)
           //        {
           //            if (sCom.IndexOf(iPort.ToString()) > -1)
           //            {
           //                break;
           //            }
           //        }

           //        iMachineNumber = Convert.ToInt32(txtMachineSN2.Text.Trim());
           //        if (iMachineNumber == 0 || iMachineNumber > 255)
           //        {
           //            MessageBox.Show("The Machine Number is invalid!", "Error");
           //            Cursor = Cursors.Default;
           //            return;
           //        }

           //        int iBaudRate = 115200;//115200 is one possible baudrate value(its value cannot be 0)
           //        bIsConnected = axCZKEM1.Connect_Com(iPort, iMachineNumber, iBaudRate);
           //    }

           if (bIsConnected == true)
           {
               btnUSBConnect.Text = "Disconnect";
               btnUSBConnect.Refresh();
               lblState.Text = "Current State:Connected";
               axCZKEM1.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
           }
           else
           {
               axCZKEM1.GetLastError(ref idwErrorCode);
               MessageBox.Show("Unable to connect the device,ErrorCode=" + idwErrorCode.ToString(), "Error");
           }

           Cursor = Cursors.Default;

       }

       private void button3_Click_1(object sender, EventArgs e)
       {

           string resultado = GetEncodedHash("abchdasd", "123");
           MessageBox.Show(resultado);

       }

string GetEncodedHash(string password, string salt)
{
   MD5 md5 = new MD5CryptoServiceProvider();
   byte [] digest = md5.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
   string base64digest = Convert.ToBase64String(digest, 0, digest.Length);
   return base64digest.Substring(0, base64digest.Length-2);
}

public static void mail(){

    string cuerpo = "";

    cuerpo =  "<table border=1 cellpadding=0 cellspacing=0>";
    cuerpo = cuerpo +"  <tr><td><table border=0 cellpadding=0 cellspacing=0><tr>";
    cuerpo = cuerpo +   "<td width=90 height=40>&nbsp;</td>";
    cuerpo = cuerpo + "<td width=257 align=center valign=middle><strong>COMPROBANTE DE REGISTRO</strong></td>";
    cuerpo = cuerpo +"<td width=99>&nbsp;</td></tr><tr><td colspan=3><table width=448 border=0>";
    cuerpo = cuerpo +"<tr> <td width=218 align=center>15.583.953-K</td><td width=206 align=center>07-11-2017 14:31:26</td>";
    cuerpo = cuerpo + "</tr></table></td></tr><tr><td>&nbsp;</td><td><div align=center>Puerto Coronel S.A</div></td><td>&nbsp;</td>";
    cuerpo = cuerpo +  "</tr><tr><td>&nbsp;</td><td><div align=center>79.895.330-3</div></td><td>&nbsp;</td></tr><tr>";
    cuerpo = cuerpo +  "<td colspan=3><div align=center>Av. Carlos Prats N°40 - Coronel - Bio Bio - Chile</div></td></tr><tr>";
    cuerpo = cuerpo +"<td colspan=3 align=center><h6>ID: 2jXkYQrMQLpMc0/13Y3r+g</h6></td></tr></table></td></tr></table>";

    using (Monitor_marca.referenciamail.EnvioMail m = new Monitor_marca.referenciamail.EnvioMail())
    {
        string respuesta = "0";

        respuesta = m.enviarMail("danielbustos86@gmail.com", "COMPROBANTE MARCA", cuerpo, "20170101");       

    } 


}


private void button4_Click(object sender, EventArgs e)
{


    //mail();

    using (Monitor_marca.referenciamail.EnvioMail em = new Monitor_marca.referenciamail.EnvioMail())
    {
        em.enviarMail("danielbustos86@gmail.com","adadda","test","20170101");

    }



}


        public static int Inserta_colamail(int idmarca,
            string asunto,
            string destinatario, 
            string cuerpo)
        {




            //   DataTable dtDatos = new DataTable();
            // Se crea un MySqlAdapter para obtener los datos de la base
            // MySqlDataAdapter mdaDatos = new MySqlDataAdapter("select id_persona  from con_huella where ID_PERSONA =?ID_PERSONA", conexion.ObtenerCOnexion());
            MySqlCommand cmd = new MySqlCommand();
            int respuesta = 0 ; 

            try{
                    cmd.Connection = conexion.ObtenerCOnexion();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "INSERTA_COLA_MAIL";

                    cmd.Parameters.AddWithValue("_idmarca", idmarca);
                    cmd.Parameters.AddWithValue("_asunto", asunto);
                    cmd.Parameters.AddWithValue("_destinatario",destinatario);
                    cmd.Parameters.AddWithValue("_cuerpo", cuerpo);
           
                    cmd.ExecuteNonQuery();

                    respuesta = 1 ; 
                }
                catch (MySqlException mysqlex)
                {
                 
                   
                    
                // ListaEstado.Items.Add("error al grabar mail:　" + mysqlex.Message.ToString());
                //ListaEstado.TopIndex = ListaEstado.Items.Count - 1;


                }

                return respuesta; 
            }

        private void button1_Click(object sender, EventArgs e)
        {
           

            notifyIcon1.Visible  = true;
            notifyIcon1.BalloonTipText  = "La aplicacion seguira en ejecucion en segundo plano";
            notifyIcon1.BalloonTipTitle = "Consultor IT";
            notifyIcon1.BalloonTipIcon  = ToolTipIcon.Info;
            notifyIcon1.ShowBalloonTip(10000);
            notifyIcon1.Icon = this.Icon;
            notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            //this.Visible = false;
            this.Hide();

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void mostrarAplicacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = !this.Visible;
            notifyIcon1.Visible = !this.Visible;

        }

        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            notifyIcon1.BalloonTipText = "La aplicacion seguira en ejecucion en segundo plano";
            notifyIcon1.BalloonTipTitle = "Consultor IT";
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.ShowBalloonTip(100000);
            notifyIcon1.Icon = this.Icon;
            notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.Visible = false;
          //  this.Hide();
        }

        private void printDocument2_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            // Create font and brush.
            Font drawFont = new Font("Arial", 6);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            // Create point for upper-left corner of drawing.

            // Draw string to screen.
            e.Graphics.DrawString("REGISTRO MARCA FALLIDA", drawFont, drawBrush, 40, 10);

            e.Graphics.DrawString("FECHA:" + Fecha_Marcacion, drawFont, drawBrush, 10, 40);

            // e.Graphics.DrawString(rut_trabajador, drawFont, drawBrush, 80, 20);

            e.Graphics.DrawString("EQUIPO:" + textBox1.Text, drawFont, drawBrush, 10, 60);
            e.Graphics.DrawString("HASH:" + hash, drawFont, drawBrush, 10, 80);
            e.HasMorePages = false;

            //   e.Graphics.DrawString(nombre_completo, drawFont, drawBrush, 80, 40);
        }


        


                
           

          

       }


      

    
}
