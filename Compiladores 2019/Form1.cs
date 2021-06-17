using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace Compiladores_2019
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
                        
        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {

            
            dataGridView1.Rows.Clear();
            button2.Enabled = false;
           this. label3.Visible = false;
          // this.Analizar();           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            label3.Text = "";
            this.Analizar();

            int contador_errores = 0;
            for (int x = 0; x < dataGridView1.RowCount; x++)
            {
                if ((dataGridView1.Rows[x].Cells[1].Value.ToString()).Equals("ERROR"))
                {
                    contador_errores += 1;
                    dataGridView1.Rows[x].DefaultCellStyle.BackColor = Color.Pink;

                }
            }
            if(contador_errores>0)
            {
                button2.Enabled = false;
               
                 this.label3.Text ="ERRORES LEXICOS TIENE: "+contador_errores;
                 this.label3.Visible = true;
            }else{
                button2.Enabled = true;
            }
           
        }



        string unir_cad = "";
        string espacio = "[' ']";
        string salto = "['\n']";
     
        string unir_string = "";
        string unir_com = "";

        int contar_columas = 1;
        int contar_lineas = 1;
        public void Analizar()
        {
            dataGridView1.Rows.Clear();

            contar_columas = 1;
            contar_lineas = 1;


            char validar_com = '0';
            char validar_cad_string = '0';

            string comentario = "@";
            string cad_string = "[\"]";
            string texto = textBox1.Text;



            foreach (char letra in texto)
            {
                string letra2 = letra.ToString();           


                if (Regex.IsMatch(letra2, cad_string))
                {
                    if (validar_cad_string.Equals('0'))
                    {
                        validar_cad_string = '1';
                    }
                    else
                    {
                        
                        dataGridView1.Rows.Add(unir_string + "\"", "CADENA", contar_lineas, contar_columas);
                        //contar_lineas += 1;
                        validar_cad_string = '0';
                        unir_string = "";


                    }
                }

                if (validar_cad_string.Equals('1'))
                {
                    unir_string = unir_string + letra2;
                }


                if (Regex.IsMatch(letra2, comentario))
                {
                    validar_com = '1';
                }

                if (validar_com.Equals('1'))
                {
                    unir_com = unir_com + letra2;

                    if (letra.Equals('\n'))
                    {

                        dataGridView1.Rows.Add(unir_com + "", "COMENTARIO", contar_lineas, contar_columas);
                        contar_lineas += 1;
                        contar_columas = 1;
                        validar_com = '0';
                        unir_com = "";


                    }

                }

                else if (validar_com.Equals('0') & validar_cad_string.Equals('0') & letra2 != "\"" & letra2 != "\r")
                {
                    if (letra2 == " " || letra2=="\n")
                    {
                        this.AnalizarPalabras();
                       
                        if (Regex.IsMatch(letra2, espacio))
                        {
                            contar_columas += 1;
                        }
                        if (Regex.IsMatch(letra2, salto))
                        {
                            contar_lineas += 1;
                            contar_columas = 1;
                        }
                    }
                    else
                    {
                        unir_cad = unir_cad + letra2;
                    }
                    
                }


            } //fin foreach

        }


        public void AnalizarPalabras()
        {


            string exp_minusculas = "[A-Z]+";


           
            //if (Regex.IsMatch(unir_cad, espacio) || Regex.IsMatch(unir_cad, salto))
            //{

                if (Regex.IsMatch(unir_cad, exp_minusculas))
                {
                    dataGridView1.Rows.Add(unir_cad + "", "ERROR", contar_lineas, contar_columas);
                    unir_cad = "";
                }
                else
                {


                    this.VerificarLexema();
                   
                   
                }
            //}
        }


        char validar_uno_mas = '0';
        public void VerificarLexema()
        {


            string[] reservado = { "inicio", "proceso", "fin", "si", "ver", "mientras", "entero", "cadena" };


            string exp_numeros = "^[0-9]+$[0-9]?";
            string exp_delimitador = "^[;|(|)|{|}]$";
            string exp_operadores = "^[+|-|/|*]$";
            string asignacion = "^#$";
            string exp_comparador = "^[<|>]$|^==$";
            string variable = "^var[(0-9)?]$";
           

            char validar_reservado = '0';



            for (int i = 0; i < 8; i++)
            {
                if (unir_cad.Equals(reservado[i]))
                {
                    dataGridView1.Rows.Add(unir_cad + "", "RESERVADO", contar_lineas, contar_columas);
                    validar_reservado = '1';
                    if (Regex.IsMatch(unir_cad, "si"))
                    {
                        validar_uno_mas = '1';
                    }
                }

            }

            if (Regex.IsMatch(unir_cad, exp_numeros))
            {
                dataGridView1.Rows.Add(unir_cad + "", "NUMERO", contar_lineas, contar_columas);
            }
            else if (Regex.IsMatch(unir_cad, exp_delimitador))
            {
                dataGridView1.Rows.Add(unir_cad + "", "DELIMITADOR", contar_lineas, contar_columas);

            }
            else if (Regex.IsMatch(unir_cad, exp_operadores))
            {
                dataGridView1.Rows.Add(unir_cad + "", "OPERADOR", contar_lineas, contar_columas);
            }
            else if (Regex.IsMatch(unir_cad, asignacion))
            {
                dataGridView1.Rows.Add(unir_cad + "", "ASIGNACION", contar_lineas, contar_columas);
            }
            else if (Regex.IsMatch(unir_cad, exp_comparador))
            {
                dataGridView1.Rows.Add(unir_cad + "", "COMPARADOR", contar_lineas, contar_columas);
            }
            else if (Regex.IsMatch(unir_cad, variable))
            {
                dataGridView1.Rows.Add(unir_cad + "", "VARIABLE", contar_lineas, contar_columas);
            }

            else if (validar_reservado.Equals('0') & unir_cad != "" & unir_cad != "\"")
            {

                dataGridView1.Rows.Add(unir_cad + "", "ERROR", contar_lineas, contar_columas);

            }
            unir_cad = "";
        }





     

        int recorrido = 0;
        string[] lexi_a_sint = null;
        string[] num_linea = null;
        string[] num_columna = null;


        string[] variables = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
        int contador_variables = 0;
        string[] tipo_variables = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

        string inicio_correcto = "0";
        string proceso_correcto = "0";
        string ya_imprimio_no_hay_proc = "0";

        
        string proceso_hay = "0";
        string fin_hay = "0";

        string[] pasar_a_c = null;
        int pasos_pasar_a_c = 0;
        ////////////////////////sintactico aqui
        public void Sintactico()
        {

           
            pasar_a_c = new string[30];
          

            int contador_comentarios = 0;
            int contador_comentarios_fila_menos = 0;

            for (int x = 0; x < dataGridView1.RowCount; x++)
            {
                if ((dataGridView1.Rows[x].Cells[1].Value.ToString()).Equals("COMENTARIO"))
                {
                   contador_comentarios_fila_menos += 1;
                    
                }
            }
          
            if(contador_comentarios_fila_menos>0)
            {
                lexi_a_sint = new string[(dataGridView1.RowCount - contador_comentarios_fila_menos) + 1];
                num_linea = new string[(dataGridView1.RowCount - contador_comentarios_fila_menos) + 1];
                num_columna = new string[(dataGridView1.RowCount - contador_comentarios_fila_menos) + 1];






                for (int s = 0; s <= (dataGridView1.RowCount-1); s++)
                {
                    if ((dataGridView1.Rows[s].Cells[1].Value.ToString()).Equals("COMENTARIO"))
                    {
                        contador_comentarios += 1;
                        
                    }
                    else
                    {
                        lexi_a_sint[s - contador_comentarios] = dataGridView1.Rows[s].Cells[0].Value.ToString();
                        num_linea[s - contador_comentarios] = dataGridView1.Rows[s].Cells[2].Value.ToString();
                        num_columna[s - contador_comentarios] = dataGridView1.Rows[s].Cells[3].Value.ToString();
                    }

                }




            }else{
                lexi_a_sint = new string[(dataGridView1.RowCount - contador_comentarios_fila_menos) + 1];
                num_linea = new string[(dataGridView1.RowCount - contador_comentarios_fila_menos) + 1];
                num_columna = new string[(dataGridView1.RowCount - contador_comentarios_fila_menos) + 1];

                for (int s = 0; s < (dataGridView1.RowCount - contador_comentarios_fila_menos); s++)
                {
                    if ((dataGridView1.Rows[s].Cells[1].Value.ToString()).Equals("COMENTARIO"))
                    {
                        contador_comentarios += 1;
                    }
                    else
                    {
                        lexi_a_sint[s - contador_comentarios] = dataGridView1.Rows[s].Cells[0].Value.ToString();
                        num_linea[s - contador_comentarios] = dataGridView1.Rows[s].Cells[2].Value.ToString();
                        num_columna[s - contador_comentarios] = dataGridView1.Rows[s].Cells[3].Value.ToString();
                    }

                }
            }
             
            lexi_a_sint[dataGridView1.RowCount - contador_comentarios_fila_menos] = "ultima linea";
            num_linea[dataGridView1.RowCount - contador_comentarios_fila_menos] = "ultima linea";
            num_columna[dataGridView1.RowCount - contador_comentarios_fila_menos] = "ultima linea";







            ////desde aqui 

            for (int recorridox = 0; recorridox < lexi_a_sint.Length; recorridox++)
            {

                if (Regex.IsMatch(lexi_a_sint[recorridox], "inicio"))
                {
                    recorridox += 1;
                    if (Regex.IsMatch(lexi_a_sint[recorridox], ";"))
                    {

                        recorridox += 1;
                        //inicio_hay = "1";
                    }
                   
                }

                if (Regex.IsMatch(lexi_a_sint[recorridox], "proceso"))
                {
                    recorridox += 1;
                    if (Regex.IsMatch(lexi_a_sint[recorridox], ";"))
                    {
                        recorridox += 1;
                        proceso_hay = "1";
                    }

                }

                if (Regex.IsMatch(lexi_a_sint[recorridox], "fin"))
                {
                    recorridox += 1;
                    if (Regex.IsMatch(lexi_a_sint[recorridox], ";"))
                    {
                        recorridox += 1;
                        fin_hay = "1";
                    }

                }
            }


            ////para aqui...



           
          

            //for (int w = 0; w < lexi_a_sint.Length-1; w++)
            //{
            //    Console.WriteLine(lexi_a_sint[w]+" linea: "+ num_linea[w]+ " columna: "+ num_columna[w]);

            //}

            for (recorrido = 0; recorrido < lexi_a_sint.Length; recorrido++)
            {
               
                if (Regex.IsMatch(lexi_a_sint[recorrido], "inicio"))
                {
                    recorrido += 1;
                    if (Regex.IsMatch(lexi_a_sint[recorrido], ";"))
                    {

                        dataGridView2.Rows.Add("CORRECTA", "INICIALIZACION --> inicio ; <-- " + " FILA: " + num_linea[recorrido]);
                         recorrido += 1;
                        inicio_correcto = "1";
                    }
                    else
                    {
                        dataGridView2.Rows.Add("ERROR", "SE ESPERABA PUNTO Y COMA" + " FILA: " + num_linea[recorrido] + " COLUMNA: " + num_columna[recorrido]);
                    }
                }

                if(inicio_correcto=="0")
                {
                    if (recorrido == 0)
                    {
                        dataGridView2.Rows.Add("ERROR","SE ESPERABA INICIALIZACION: --> inicio ; <-- " + "ANTES DE FILA: " + num_linea[recorrido]);
                       
                    }
                    //Console.Write("SE ESPERABA INICIALIZACION: inicio ;" + "antes de fila: " + num_linea[recorrido] +"\n");
                    recorrido_sum = recorrido;
                    this.estruc_var_cadena();
                    recorrido = recorrido_sum;

                    recorrido_sum = recorrido;
                    this.estruc_var_entera();
                    recorrido = recorrido_sum;
                }
                 if (inicio_correcto == "1")
                {
                    recorrido_sum = recorrido;
                    this.estruc_var_cadena();
                   

                    recorrido_sum = recorrido;
                    this.estruc_var_entera();
                    
                }
               

             if (Regex.IsMatch(lexi_a_sint[recorrido], "proceso"))
                {
                  
                    recorrido += 1;
                    if (Regex.IsMatch(lexi_a_sint[recorrido], ";"))
                    {

                        dataGridView2.Rows.Add("CORRECTA", "INICIALIZACION --> proceso ; <-- " + "FILA: " + num_linea[recorrido]);
                       
                        recorrido += 1;
                        proceso_correcto = "1";
                    }
                    else
                    {
                        dataGridView2.Rows.Add("ERROR", "SE ESPERABA PUNTO Y COMA" + " FILA: " + num_linea[recorrido] + " COLUMNA: " + num_columna[recorrido]);
                    }
                }


                if (proceso_correcto == "1")
                {
                    recorrido_sum = recorrido;
                    this.estruc_ver();
                    recorrido = recorrido_sum;

                    recorrido_sum = recorrido;
                    this.estruc_si();
                    recorrido = recorrido_sum;

                    recorrido_sum = recorrido;
                    this.estruc_mientras();
                    recorrido = recorrido_sum;

                    recorrido_sum = recorrido;
                    this.estruc_var_cadenax();


                    recorrido_sum = recorrido;
                    this.estruc_var_enterax();
                }

                if (proceso_hay == "1" & proceso_correcto == "0")
                {

                    recorrido_sum = recorrido;
                    this.estruc_verx();
                    recorrido = recorrido_sum;

                    recorrido_sum = recorrido;
                    this.estruc_six();
                    recorrido = recorrido_sum;

                    recorrido_sum = recorrido;
                    this.estruc_mientrasx();
                    recorrido = recorrido_sum;

                }
                 else if (proceso_correcto == "0")
                {
                    recorrido_sum = recorrido;
                    this.estruc_ver();
                    recorrido = recorrido_sum;

                    recorrido_sum = recorrido;
                    this.estruc_si();
                    recorrido = recorrido_sum;

                    recorrido_sum = recorrido;
                    this.estruc_mientras();
                    recorrido = recorrido_sum;
                }


                if (Regex.IsMatch(lexi_a_sint[recorrido], "fin"))
                {

                    recorrido += 1;
                    if (Regex.IsMatch(lexi_a_sint[recorrido], ";"))
                    {
                        fin_hay = "1";
                        dataGridView2.Rows.Add("CORRECTA", "FINALIZACION --> fin ; <--" + " FILA: " + num_linea[recorrido]);
                        recorrido += 1;
                       
                    }
                    else
                    {
                      //  dataGridView2.Rows.Add("ERROR", "SE ESPERABA PUNTO Y COMA" + " FILA: " + num_linea[recorrido] + " COLUMNA: " + num_columna[recorrido]);
                    }
                }

                if (Regex.IsMatch(lexi_a_sint[recorrido], "ultima linea"))
                {
                    if (lexi_a_sint[recorrido - 2].Equals("fin") & lexi_a_sint[recorrido - 1].Equals(";"))
                    {
                      //  dataGridView2.Rows.Add("CORRECTA", "FINALIZACION --> fin ; <--" + " FILA: " + num_linea[recorrido]);
                    }
                    else
                    {
                        if (fin_hay == "1")
                        {
                            dataGridView2.Rows.Add("ERROR", "NO PUEDE ESCRIBIR DESPUES DE: --> fin ; <--" + " " + num_linea[recorrido]);
                           
                        }
                        else
                        {
                            dataGridView2.Rows.Add("ERROR", "SE ESPERABA FINALIZACION: --> fin ; <--" + " " + num_linea[recorrido]);
                            
                        }
                    }
                }




            }//fin de for



        }//fin public void Sintactico()



        int contador_error_compilar = 0;

        private void button2_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < pasos_pasar_a_c; x++)
            {

                pasar_a_c[x] = "";


            }
            pasos_pasar_a_c = 0;
            dataGridView2.Rows.Clear();
            for (int i = 0; i < variables.Length; i++ )
            {
                variables[i] = "0";
            }
            inicio_correcto = "0";
            proceso_correcto = "0";
            proceso_hay = "0";
            fin_hay = "0";
            contador_error_compilar = 0;
            contador_variables = 0;
           this.Sintactico();


           for (int x = 0; x < dataGridView2.RowCount; x++)
           {
               if ((dataGridView2.Rows[x].Cells[0].Value.ToString()).Equals("ERROR"))
               {
                  
                   dataGridView2.Rows[x].DefaultCellStyle.BackColor = Color.Pink;
                   contador_error_compilar += 1;
               }
           }

            if(contador_error_compilar==0)
            {




                using (StreamWriter writer = new StreamWriter("C:\\Users\\dilci\\Desktop\\utesa2020\\compiladores\\analizador sintactico\\DOOES en C++.txt", false)) 

                   

                for(int x=0; x<pasos_pasar_a_c; x++)
                {
                    if(x==0)
                    {
                        writer.WriteLine("#include <iostream>");
                        writer.WriteLine("using namespace std;");
                        writer.WriteLine("int main() {");
                        writer.WriteLine("");
                        writer.WriteLine("");
                        writer.WriteLine("");

                    }
                   
                      //  Console.Write(pasar_a_c[x]);
                    writer.WriteLine(pasar_a_c[x].ToString());


                    if (x == pasos_pasar_a_c-1)
                    {
                        writer.WriteLine("");
                        writer.WriteLine("");
                        writer.WriteLine("return 0;");
                        writer.WriteLine("}");
                       
                    }
                }


               
            }

        }

       int recorrido_sum =0;
        public void estruc_var_entera()
        {
            
            //inicia reconocimento de declaracion de variable entera
            string existe = "no";
            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "entero"))
            {
                recorrido_sum += 1;
                if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^var[(0-9)?]$"))
                {
                    recorrido_sum += 1;
                    if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "#"))
                    {
                        recorrido_sum += 1;
                        if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^[0-9]+$[0-9]?"))
                        {
                            recorrido_sum += 1;
                            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], ";"))
                            {
                                for (int i = 0; i < contador_variables + 1; i++ )
                                {
                                    if(variables[i].Equals(lexi_a_sint[recorrido_sum-3]))
                                    {
                                        existe = "si";
                                    }
                                }
                                if(existe=="si")
                                {
                                    dataGridView2.Rows.Add("ERROR", "NOMBRE DE VARIABLE YA EXISTE" + " FILA: " + num_linea[recorrido]);
                                }
                                if(existe=="no")
                                {
                                    variables[contador_variables] = lexi_a_sint[recorrido_sum - 3];
                                    tipo_variables[contador_variables] = "numero";
                                    contador_variables += 1;
                                    dataGridView2.Rows.Add("CORRECTA", "ASIGNACION DE VARIABLE ENTERA" + " FILA: " + num_linea[recorrido]);

                                    pasar_a_c[pasos_pasar_a_c] = "int " + lexi_a_sint[recorrido_sum - 3] + " = " + lexi_a_sint[recorrido_sum - 1] + ";" +"\n";
                                    pasos_pasar_a_c += 1;
                                }
                               
                               
                               
                            }
                            else
                            {

                                dataGridView2.Rows.Add("ERROR", "SE ESPERABA PUNTO Y COMA" + " FILA: " + num_linea[recorrido] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                            }

                        }
                        else
                        {
                            dataGridView2.Rows.Add("ERROR", "SE ESPERABA UN NUMERO" + " FILA: " + num_linea[recorrido] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                        }

                    }
                    else
                    {
                        dataGridView2.Rows.Add("ERROR", "SE ESPERABA ASIGNADOR" + " FILA: " + num_linea[recorrido] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                    }
                }
                else
                {
                    dataGridView2.Rows.Add("ERROR", "SE ESPERABA VARIABLE" + " FILA: " + num_linea[recorrido] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                }
                recorrido = recorrido_sum;
            }
            //finaliza reconocimento de declaracion de variable entera
            } //fin public void estruc_var_numero




                 public void estruc_var_enterax()
        {
            
            //inicia reconocimento de declaracion de variable enterax
            
            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "entero"))
            {
                recorrido_sum += 1;
                if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^var[(0-9)?]$"))
                {
                    recorrido_sum += 1;
                    if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "#"))
                    {
                        recorrido_sum += 1;
                        if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^[0-9]+$[0-9]?"))
                        {
                            recorrido_sum += 1;
                            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], ";"))
                            {
                                 dataGridView2.Rows.Add("ERROR", "DECLARACION --> entero <--DEBE IR ANTES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido]);
                            }
                            else
                            {

                                 dataGridView2.Rows.Add("ERROR", "DECLARACION --> entero <--DEBE IR ANTES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido]); recorrido_sum -= 1;
                            }

                        }
                        else
                        {
                            dataGridView2.Rows.Add("ERROR", "DECLARACION --> entero <--DEBE IR ANTES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido]); recorrido_sum -= 1;
                        }

                    }
                    else
                    {
                        dataGridView2.Rows.Add("ERROR", "DECLARACION --> entero <--DEBE IR ANTES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido]); recorrido_sum -= 1;
                    }
                }
                else
                {
                    dataGridView2.Rows.Add("ERROR", "DECLARACION --> entero <--DEBE IR ANTES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido]); recorrido_sum -= 1;
                }
                recorrido = recorrido_sum;
            }
            //finaliza reconocimento de declaracion de variable enterax

        } //fin public void estruc_var_numero

        public void estruc_var_cadena()
        {
         
            //inicia reconocimento de declaracion de variable cadena
            string existe = "no";
            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "cadena"))
            {
                recorrido_sum += 1;
                if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^var[(0-9)?]$"))
                {
                    recorrido_sum += 1;
                    if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "#"))
                    {
                        recorrido_sum += 1;
                        if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^\".*\"$"))
                        {
                            recorrido_sum += 1;
                            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], ";"))
                            {
                                for (int i = 0; i < contador_variables + 1; i++)
                                {
                                    if (variables[i].Equals(lexi_a_sint[recorrido_sum - 3]))
                                    {
                                        existe = "si";
                                    }
                                }
                                if (existe == "si")
                                {
                                    dataGridView2.Rows.Add("ERROR", "NOMBRE DE VARIABLE YA EXISTE" + " FILA: " + num_linea[recorrido]);
                                }
                                if (existe == "no")
                                {
                                    variables[contador_variables] = lexi_a_sint[recorrido_sum - 3];
                                    tipo_variables[contador_variables] = "cadena";
                                    contador_variables += 1;
                                    dataGridView2.Rows.Add("CORRECTA", "ASIGNACION DE VARIABLE CADENA" + " FILA: " + num_linea[recorrido]);
                                    pasar_a_c[pasos_pasar_a_c] = "string " + lexi_a_sint[recorrido_sum - 3] + " = " + lexi_a_sint[recorrido_sum - 1] + ";" + "\n";
                                    pasos_pasar_a_c += 1;
                                }
                            }
                            else
                            {
                                dataGridView2.Rows.Add("ERROR", "SE ESPERABA PUNTO Y COMA" + " FILA: " + num_linea[recorrido] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                            }

                        }
                        else
                        {
                            dataGridView2.Rows.Add("ERROR", "SE ESPERABA UNA CADENA" + " FILA: " + num_linea[recorrido] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                        }

                    }
                    else
                    {
                        dataGridView2.Rows.Add("ERROR", "SE ESPERABA ASIGNADOR" + " FILA: " + num_linea[recorrido] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                    }
                }
                else
                {
                    dataGridView2.Rows.Add("ERROR", "SE ESPERABA VARIABLE" + " FILA: " + num_linea[recorrido] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                }
                recorrido = recorrido_sum;
            }
            //finaliza reconocimento de declaracion de variable cadena

        }//fin public void estruc_var_cadena



        public void estruc_var_cadenax()
        {

            //inicia reconocimento de declaracion de variable cadenax

            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "cadena"))
            {
                recorrido_sum += 1;
                if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^var[(0-9)?]$"))
                {
                    recorrido_sum += 1;
                    if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "#"))
                    {
                        recorrido_sum += 1;
                        if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^\".*\"$"))
                        {
                            recorrido_sum += 1;
                            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], ";"))
                            {
                                dataGridView2.Rows.Add("ERROR", "DECLARACION --> cadena <--DEBE IR ANTES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido]);
                            }
                            else
                            {
                                dataGridView2.Rows.Add("ERROR", "DECLARACION --> cadena <--DEBE IR ANTES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido]); recorrido_sum -= 1;
                            }

                        }
                        else
                        {
                            dataGridView2.Rows.Add("ERROR", "DECLARACION --> cadena <--DEBE IR ANTES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido]); recorrido_sum -= 1;
                        }

                    }
                    else
                    {
                        dataGridView2.Rows.Add("ERROR", "DECLARACION --> cadena <--DEBE IR ANTES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido]); recorrido_sum -= 1;
                    }
                }
                else
                {
                    dataGridView2.Rows.Add("ERROR", "DECLARACION --> cadena <--DEBE IR ANTES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido]); recorrido_sum -= 1;
                }
                recorrido = recorrido_sum;
            }
            //finaliza reconocimento de declaracion de variable cadenax

        }//fin public void estruc_var_cadenax




        public void estruc_ver()
        {
            //inicia reconocimento de ver
   
            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "ver"))
            {
                if ((lexi_a_sint[recorrido_sum - 2] != "proceso" || lexi_a_sint[recorrido_sum - 1] != "proceso") & proceso_correcto == "0" & ya_imprimio_no_hay_proc=="0")
                {
                    dataGridView2.Rows.Add("ERROR", "SE ESPERABA INICIALIZACION: --> proceso ; <--" + " ANTES DE FILA: " + num_linea[recorrido] + "\n");
                    ya_imprimio_no_hay_proc = "1";
                }
                recorrido_sum += 1;
                        if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^\".*\"$"))
                        {
                            recorrido_sum += 1;
                            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], ";"))
                            {
                                dataGridView2.Rows.Add("CORRECTA", "IMPRIMIR EN PANTALLA VER" + " FILA: " + num_linea[recorrido_sum]);
                                pasar_a_c[pasos_pasar_a_c] = " cout << " + lexi_a_sint[recorrido_sum - 1] + " << endl" + ";" + "\n";
                                pasos_pasar_a_c += 1;
                            }
                            else
                            {
                                dataGridView2.Rows.Add("ERROR", "SE ESPERABA PUNTO Y COMA" + " FILA: " + num_linea[recorrido] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                            }

                        }
                        else
                        {
                            dataGridView2.Rows.Add("ERROR", "SE ESPERABA UNA CADENA" + " FILA: " + num_linea[recorrido] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                        }

                recorrido = recorrido_sum;
            }
            //finaliza reconocimento de ver
           // recorrido_sum -= 1;
        }//fin public void estruc_ver



        public void estruc_verx()
        {
            //inicia reconocimento de verx

            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "ver"))
            {
                //if ((lexi_a_sint[recorrido_sum - 2] != "proceso" || lexi_a_sint[recorrido_sum - 1] != "proceso") & proceso_correcto == "0" & ya_imprimio_no_hay_proc == "0")
                //{
                //    dataGridView2.Rows.Add("ERROR", "SE ESPERABA INICIALIZACION: --> proceso ; <--" + " ANTES DE FILA: " + num_linea[recorrido] + "\n");
                //    ya_imprimio_no_hay_proc = "1";
                //}
                recorrido_sum += 1;
                if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^\".*\"$"))
                {
                    recorrido_sum += 1;
                    if (Regex.IsMatch(lexi_a_sint[recorrido_sum], ";"))
                    {
                        dataGridView2.Rows.Add("ERROR", "ESTRUCTURA --> ver <--DEBE IR DESPUES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido]);
                    }
                    else
                    {
                        dataGridView2.Rows.Add("ERROR", "ESTRUCTURA --> ver <--DEBE IR DESPUES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido]); recorrido_sum -= 1;
                    }

                }
                else
                {
                    dataGridView2.Rows.Add("ERROR", "ESTRUCTURA --> ver <--DEBE IR DESPUES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido]); recorrido_sum -= 1;
                }

                recorrido = recorrido_sum;
            }
            //finaliza reconocimento de ver
            // recorrido_sum -= 1;
        }//fin public void estruc_verx

        public void estruc_si()
        {
            //inicia reconocimento de si

            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "si"))
            {
                if ((lexi_a_sint[recorrido_sum - 2] != "proceso" || lexi_a_sint[recorrido_sum - 1] != "proceso") & proceso_correcto == "0" & ya_imprimio_no_hay_proc == "0")
                {
                    dataGridView2.Rows.Add("ERROR", "SE ESPERABA INICIALIZACION: --> proceso ; <--" + " ANTES DE FILA: " + num_linea[recorrido_sum]);
                    ya_imprimio_no_hay_proc = "1";
                }
                recorrido_sum += 1;
                if (lexi_a_sint[recorrido_sum].Equals("("))
                {
                    recorrido_sum += 1;
                    this.estruc_comparacion();
                    recorrido_sum += 1;
                    if (lexi_a_sint[recorrido_sum].Equals(")"))
                    {
                        recorrido_sum += 1;
                        if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "{"))
                        {
                            recorrido_sum += 1;
                            this.estruc_ver_dentro_de_si();
                            recorrido_sum += 1;
                            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "}"))
                            {
                                dataGridView2.Rows.Add("CORRECTA", "SI ( ) { }" + " FINALIZACION FILA: " + num_linea[recorrido_sum]);
                                //pasar_a_c[pasos_pasar_a_c] = " if ( ";

                                //pasos_pasar_a_c += 1;
                            }
                            else
                            {
                                dataGridView2.Rows.Add("ERROR", "SE ESPERABA CIERRE DE LLAVE" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                            }

                        }
                        else
                        {
                            dataGridView2.Rows.Add("ERROR", "SE ESPERABA APERTURA DE LLAVE" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                        }

                    }
                    else
                    {
                        dataGridView2.Rows.Add("ERROR", "SE ESPERABA CIERRE DE PARENTESIS" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                    }
                }
                else
                {
                    dataGridView2.Rows.Add("ERROR", "SE ESPERABA APERTURA DE PARENTESIS" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                }

                    recorrido = recorrido_sum;
                }
             //finaliza reconocimento de si

        }//fin public void estruc_si





        public void estruc_six()
        {
            //inicia reconocimento de six

            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "si"))
            {
                //if ((lexi_a_sint[recorrido_sum - 2] != "proceso" || lexi_a_sint[recorrido_sum - 1] != "proceso") & proceso_correcto == "0" & ya_imprimio_no_hay_proc == "0")
                //{
                //    dataGridView2.Rows.Add("ERROR", "SE ESPERABA INICIALIZACION: --> proceso ; <--" + " ANTES DE FILA: " + num_linea[recorrido_sum]);
                //    ya_imprimio_no_hay_proc = "1";
                //}
                recorrido_sum += 1;
                if (lexi_a_sint[recorrido_sum].Equals("("))
                {
                    recorrido_sum += 1;
                    this.estruc_comparacion();
                    recorrido_sum += 1;
                    if (lexi_a_sint[recorrido_sum].Equals(")"))
                    {
                        recorrido_sum += 1;
                        if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "{"))
                        {
                            recorrido_sum += 1;
                            this.estruc_ver_dentro_de_si();
                            recorrido_sum += 1;
                            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "}"))
                            {
                                dataGridView2.Rows.Add("ERROR", "ESTRUCTURA --> si <--DEBE IR DESPUES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido_sum]);
                            }
                            else
                            {
                                 dataGridView2.Rows.Add("ERROR", "ESTRUCTURA --> si <--DEBE IR DESPUES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido_sum]); recorrido_sum -= 1;
                            }

                        }
                        else
                        {
                            dataGridView2.Rows.Add("ERROR", "ESTRUCTURA --> si <--DEBE IR DESPUES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido_sum]); recorrido_sum -= 1;
                        }

                    }
                    else
                    {
                        dataGridView2.Rows.Add("ERROR", "ESTRUCTURA --> si <--DEBE IR DESPUES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido_sum]); recorrido_sum -= 1;
                    }
                }
                else
                {
                    dataGridView2.Rows.Add("ERROR", "ESTRUCTURA --> si <--DEBE IR DESPUES DE --> proceso ; <-- " + "FILA: " + num_linea[recorrido_sum]); recorrido_sum -= 1;
                }

                recorrido = recorrido_sum;
            }
            //finaliza reconocimento de six

        }//fin public void estruc_six









        public void estruc_mientras()
        {
            //inicia reconocimento de mientras

            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "mientras"))
            {
                if ((lexi_a_sint[recorrido_sum - 2] != "proceso" || lexi_a_sint[recorrido_sum - 1] != "proceso") & proceso_correcto == "0" & ya_imprimio_no_hay_proc == "0")
                {
                    dataGridView2.Rows.Add("ERROR", "SE ESPERABA INICIALIZACION: --> proceso ; <--" + " ANTES DE FILA: " + num_linea[recorrido_sum]);
                    ya_imprimio_no_hay_proc = "1";
                }
                recorrido_sum += 1;
                if (lexi_a_sint[recorrido_sum].Equals("("))
                {
                    recorrido_sum += 1;
                    this.estruc_comparacion_mientras();
                    recorrido_sum += 1;
                    if (lexi_a_sint[recorrido_sum].Equals(")"))
                    {
                        recorrido_sum += 1;
                        if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "{"))
                        {
                            recorrido_sum += 1;
                            this.estruc_ver_dentro_de_si();
                            recorrido_sum += 1;
                            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "}"))
                            {
                                dataGridView2.Rows.Add("CORRECTA", "MIENTRAS ( ) { }" + " FINALIZACION FILA: " + num_linea[recorrido_sum]);
                            }
                            else
                            {
                                dataGridView2.Rows.Add("ERROR", "SE ESPERABA CIERRE DE LLAVE" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                            }

                        }
                        else
                        {
                            dataGridView2.Rows.Add("ERROR", "SE ESPERABA APERTURA DE LLAVE" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                        }

                    }
                    else
                    {
                        dataGridView2.Rows.Add("ERROR", "SE ESPERABA CIERRE DE PARENTESIS" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                    }
                }
                else
                {
                    dataGridView2.Rows.Add("ERROR", "SE ESPERABA APERTURA DE PARENTESIS" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                }

                recorrido = recorrido_sum;
            }
            //finaliza reconocimento de Mientras

        }//fin public void estruc_mientras







        public void estruc_mientrasx()
        {
            //inicia reconocimento de mientras

            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "mientras"))
            {
                //if ((lexi_a_sint[recorrido_sum - 2] != "proceso" || lexi_a_sint[recorrido_sum - 1] != "proceso") & proceso_correcto == "0" & ya_imprimio_no_hay_proc == "0")
                //{
                //    dataGridView2.Rows.Add("ERROR", "SE ESPERABA INICIALIZACION: --> proceso ; <--" + " ANTES DE FILA: " + num_linea[recorrido_sum]);
                //    ya_imprimio_no_hay_proc = "1";
                //}
                recorrido_sum += 1;
                if (lexi_a_sint[recorrido_sum].Equals("("))
                {
                    recorrido_sum += 1;
                    this.estruc_comparacion_mientras();
                    recorrido_sum += 1;
                    if (lexi_a_sint[recorrido_sum].Equals(")"))
                    {
                        recorrido_sum += 1;
                        if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "{"))
                        {
                            recorrido_sum += 1;
                            this.estruc_ver_dentro_de_si();
                            recorrido_sum += 1;
                            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "}"))
                            {
                                dataGridView2.Rows.Add("CORRECTA", "MIENTRAS ( ) { }" + " FINALIZACION FILA: " + num_linea[recorrido_sum]);
                            }
                            else
                            {
                                dataGridView2.Rows.Add("ERROR", "SE ESPERABA CIERRE DE LLAVE" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                            }

                        }
                        else
                        {
                            dataGridView2.Rows.Add("ERROR", "SE ESPERABA APERTURA DE LLAVE" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                        }

                    }
                    else
                    {
                        dataGridView2.Rows.Add("ERROR", "SE ESPERABA CIERRE DE PARENTESIS" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                    }
                }
                else
                {
                    dataGridView2.Rows.Add("ERROR", "SE ESPERABA APERTURA DE PARENTESIS" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                }

                recorrido = recorrido_sum;
            }
            //finaliza reconocimento de Mientrasx

        }//fin public void estruc_mientrax





        public void estruc_ver_dentro_de_si()
        {
            //inicia reconocimento de ver dentro de si

            if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "ver"))
            {
                if ((lexi_a_sint[recorrido_sum - 2] != "proceso" || lexi_a_sint[recorrido_sum - 1] != "proceso") & proceso_correcto == "0" & ya_imprimio_no_hay_proc == "0")
                {
                    dataGridView2.Rows.Add("ERROR", "SE ESPERABA INICIALIZACION: --> proceso ; <--" + " ANTES DE FILA: " + num_linea[recorrido_sum]);
                    ya_imprimio_no_hay_proc = "1";
                }
                recorrido_sum += 1;
                if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^\".*\"$"))
                {
                    recorrido_sum += 1;
                    if (Regex.IsMatch(lexi_a_sint[recorrido_sum], ";"))
                    {
                        dataGridView2.Rows.Add("CORRECTA", "IMPRIMIR EN PANTALLA VER" + " FILA: " + num_linea[recorrido_sum]);
                        pasar_a_c[pasos_pasar_a_c] = "{ " + "\n";
                        pasos_pasar_a_c += 1;
                        pasar_a_c[pasos_pasar_a_c] = " cout << " + lexi_a_sint[recorrido_sum - 1] + " << endl" + ";"+ "\n";
                        pasos_pasar_a_c += 1;
                        pasar_a_c[pasos_pasar_a_c] = "} " + "\n";
                        pasos_pasar_a_c += 1;
                    }
                    else
                    {
                        dataGridView2.Rows.Add("ERROR", "SE ESPERABA PUNTO Y COMA" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                    }

                }
                else
                {
                    dataGridView2.Rows.Add("ERROR", "SE ESPERABA UNA CADENA" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                }

                recorrido = recorrido_sum;
            }
            else
            {
                pasar_a_c[pasos_pasar_a_c] = "{ " + "\n";
                pasos_pasar_a_c += 1;
                
                pasar_a_c[pasos_pasar_a_c] = "} " + "\n";
                pasos_pasar_a_c += 1;
                recorrido_sum -= 1;
            }
            
           
        }//fin public void estruc_ver_dentro_de_si

       public void estruc_comparacion()
       {
           //inicia reconocimento de comparacion
           string existe = "no";
           string existe2 = "no";
           string tipo = "ninguno";
           string tipo2 = "ninguno";
         
           if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^var[(0-9)?]$|^[0-9]+$[0-9]?|^\".*\"$"))
           {
               if(Regex.IsMatch(lexi_a_sint[recorrido_sum], "^var[(0-9)?]$"))
               {
                   for (int i = 0; i < contador_variables + 1; i++)
                   {
                       if (variables[i].Equals(lexi_a_sint[recorrido_sum]))
                       {
                           tipo = tipo_variables[i];
                       }
                   }
               }else if(Regex.IsMatch(lexi_a_sint[recorrido_sum], "^[0-9]+$[0-9]?"))
               {
                   tipo = "numero";
                   existe = "si";
               }
               else if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^\".*\"$"))
               {
                   tipo = "cadena";
                   existe = "si";
               }


               if(Regex.IsMatch(lexi_a_sint[recorrido_sum], "^var[(0-9)?]$"))
               {
                   for (int i = 0; i < contador_variables + 1; i++)
                   {
                       if (variables[i].Equals(lexi_a_sint[recorrido_sum]))
                       {
                           existe = "si";
                       }
                   }
                   if (existe == "si")
                   {
                       
                   }
                   if (existe == "no")
                   {
                       dataGridView2.Rows.Add("ERROR", "NOMBRE DE VARIABLE NO DECLARADA" + " FILA: " + num_linea[recorrido] + " COLUMNA: " + num_columna[recorrido_sum]);
                   }
               }



               recorrido_sum += 1;
               if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^[<|>]$|^==$"))
               {
                   recorrido_sum += 1;
                   if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^var[(0-9)?]$|^[0-9]+$[0-9]?|^\".*\"$"))
                   {
                       if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^var[(0-9)?]$"))
                       {
                           for (int i = 0; i < contador_variables + 1; i++)
                           {
                               if (variables[i].Equals(lexi_a_sint[recorrido_sum]))
                               {
                                   tipo2 = tipo_variables[i];
                               }
                           }
                       }
                       else if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^[0-9]+$[0-9]?"))
                       {
                           tipo2 = "numero";
                           existe2 = "si";
                       }
                       else if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^\".*\"$"))
                       {
                           tipo2 = "cadena";
                           existe2 = "si";
                       }

                       if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^var[(0-9)?]$"))
                       {
                           for (int i = 0; i < contador_variables + 1; i++)
                           {
                               if (variables[i].Equals(lexi_a_sint[recorrido_sum]))
                               {
                                   existe2 = "si";
                               }
                           }
                           if (existe2 == "si")
                           {

                           }
                           if (existe2 == "no")
                           {
                               dataGridView2.Rows.Add("ERROR", "NOMBRE DE VARIABLE NO DECLARADA" + " FILA: " + num_linea[recorrido] + " COLUMNA: " + num_columna[recorrido_sum]);
                           }
                       }

                       if (existe == "no" || existe2 == "no")
                       {

                       }
                       else
                       {

                           if (tipo == tipo2)
                           {
                               dataGridView2.Rows.Add("CORRECTA", " COMPARACION" + " FILA: " + num_linea[recorrido_sum]);
                               pasar_a_c[pasos_pasar_a_c] = "if ( " + lexi_a_sint[recorrido_sum - 2] + " " + lexi_a_sint[recorrido_sum - 1] + " " + lexi_a_sint[recorrido_sum] + " )" + "\n";
                               pasos_pasar_a_c += 1;
                               
                           }
                           else
                           {
                               dataGridView2.Rows.Add("ERROR", " DATOS DIFERENTES EN COMPARACION " + " FILA: " + num_linea[recorrido_sum]);
                           }


                       }
                      //
                   }
                   else
                   {
                       dataGridView2.Rows.Add("ERROR", "SE ESPERABA VARIABLE, CADENA O NUMERO" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                   }

               }
               else
               {
                   dataGridView2.Rows.Add("ERROR", "SE ESPERABA UN COMPARADOR" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
               }

              // recorrido = recorrido_sum;
           }
           else
           {
               dataGridView2.Rows.Add("ERROR", "SE ESPERABA UNA VARIABLE, CADENA O NUMERO" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
           }
           
       }//finaliza reconocimento de comparacion





       public void estruc_comparacion_mientras()
       {
           //inicia reconocimento de comparacionmientras
           string existe = "no";
           string existe2 = "no";
           string tipo = "ninguno";
           string tipo2 = "ninguno";

           if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^var[(0-9)?]$|^[0-9]+$[0-9]?|^\".*\"$"))
           {
               if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^var[(0-9)?]$"))
               {
                   for (int i = 0; i < contador_variables + 1; i++)
                   {
                       if (variables[i].Equals(lexi_a_sint[recorrido_sum]))
                       {
                           tipo = tipo_variables[i];
                       }
                   }
               }
               else if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^[0-9]+$[0-9]?"))
               {
                   tipo = "numero";
                   existe = "si";
               }
               else if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^\".*\"$"))
               {
                   tipo = "cadena";
                   existe = "si";
               }


               if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^var[(0-9)?]$"))
               {
                   for (int i = 0; i < contador_variables + 1; i++)
                   {
                       if (variables[i].Equals(lexi_a_sint[recorrido_sum]))
                       {
                           existe = "si";
                       }
                   }
                   if (existe == "si")
                   {

                   }
                   if (existe == "no")
                   {
                       dataGridView2.Rows.Add("ERROR", "NOMBRE DE VARIABLE NO DECLARADA" + " FILA: " + num_linea[recorrido] + " COLUMNA: " + num_columna[recorrido_sum]);
                   }
               }



               recorrido_sum += 1;
               if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^[<|>]$|^==$"))
               {
                   recorrido_sum += 1;
                   if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^var[(0-9)?]$|^[0-9]+$[0-9]?|^\".*\"$"))
                   {
                       if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^var[(0-9)?]$"))
                       {
                           for (int i = 0; i < contador_variables + 1; i++)
                           {
                               if (variables[i].Equals(lexi_a_sint[recorrido_sum]))
                               {
                                   tipo2 = tipo_variables[i];
                               }
                           }
                       }
                       else if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^[0-9]+$[0-9]?"))
                       {
                           tipo2 = "numero";
                           existe2 = "si";
                       }
                       else if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^\".*\"$"))
                       {
                           tipo2 = "cadena";
                           existe2 = "si";
                       }

                       if (Regex.IsMatch(lexi_a_sint[recorrido_sum], "^var[(0-9)?]$"))
                       {
                           for (int i = 0; i < contador_variables + 1; i++)
                           {
                               if (variables[i].Equals(lexi_a_sint[recorrido_sum]))
                               {
                                   existe2 = "si";
                               }
                           }
                           if (existe2 == "si")
                           {

                           }
                           if (existe2 == "no")
                           {
                               dataGridView2.Rows.Add("ERROR", "NOMBRE DE VARIABLE NO DECLARADA" + " FILA: " + num_linea[recorrido] + " COLUMNA: " + num_columna[recorrido_sum]);
                           }
                       }

                       if (existe == "no" || existe2 == "no")
                       {

                       }
                       else
                       {

                           if (tipo == tipo2)
                           {
                               dataGridView2.Rows.Add("CORRECTA", " COMPARACION" + " FILA: " + num_linea[recorrido_sum]);
                               pasar_a_c[pasos_pasar_a_c] = "while ( " + lexi_a_sint[recorrido_sum - 2] + " " + lexi_a_sint[recorrido_sum - 1] + " " + lexi_a_sint[recorrido_sum] + " )" + "\n";
                               pasos_pasar_a_c += 1;

                           }
                           else
                           {
                               dataGridView2.Rows.Add("ERROR", " DATOS DIFERENTES EN COMPARACION " + " FILA: " + num_linea[recorrido_sum]);
                           }


                       }
                       //
                   }
                   else
                   {
                       dataGridView2.Rows.Add("ERROR", "SE ESPERABA VARIABLE, CADENA O NUMERO" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
                   }

               }
               else
               {
                   dataGridView2.Rows.Add("ERROR", "SE ESPERABA UN COMPARADOR" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
               }

               // recorrido = recorrido_sum;
           }
           else
           {
               dataGridView2.Rows.Add("ERROR", "SE ESPERABA UNA VARIABLE, CADENA O NUMERO" + " FILA: " + num_linea[recorrido_sum] + " COLUMNA: " + num_columna[recorrido_sum]); recorrido_sum -= 1;
           }

       }//finaliza reconocimento de comparacion


       private void Form1_Load(object sender, EventArgs e)
       {

       }
      
       private void richTextBox1_TextChanged(object sender, EventArgs e)
       {

          
       }

       private void button3_Click(object sender, EventArgs e)
       {
          
               
           
       }
        
    }
}
