using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace babyFoot2
{
    public partial class Insertion : Form
    {
        Form1 form1 = new Form1();
        int nombreBalle = 1;
        public Insertion()
        {
            InitializeComponent();
            form1.Owner = this;
        }

        //recuperer le reste de monaie de j1
        private decimal[] getRestArgent(int idJ1, int idJ2)
        {
            Connexion con = new Connexion();
            SqlConnection connection = Connexion.connexionMysql();
            SqlCommand command = new SqlCommand("select j1.vola+sum(gain.gain1),j2.vola+sum(gain.gain2) from gain join joueur as j1 on gain.idJ1=j1.idJoueur join joueur as j2 on gain.idJ2=j2.idJoueur where gain.idJ1=@idJ1 and gain.idJ2=@idJ2  group by gain.idJ1,gain.idJ2,j1.vola,j2.vola", connection);
            command.Parameters.AddWithValue("@idJ1", idJ1);
            command.Parameters.AddWithValue("@idJ2", idJ2);
            
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            decimal somme1=new decimal();
            decimal somme2 = new decimal();

            while (reader.Read())
            {
                somme1 = reader.GetDecimal(0);
                somme2 = reader.GetDecimal(1);
            }

            decimal[] result = new decimal[2];

            result[0] = somme1;
            result[1] = somme2;

            return result;
        }

        private void Insertion_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'babyFootDataSet4.gain' table. You can move, or remove it, as needed.
            this.gainTableAdapter3.Fill(this.babyFootDataSet4.gain);
            // TODO: This line of code loads data into the 'babyFootDataSet3.gain' table. You can move, or remove it, as needed.
            this.gainTableAdapter2.Fill(this.babyFootDataSet3.gain);
            // TODO: This line of code loads data into the 'babyFootDataSet2.gain' table. You can move, or remove it, as needed.
            this.gainTableAdapter1.Fill(this.babyFootDataSet2.gain);
            // TODO: This line of code loads data into the 'babyFootDataSet1.gain' table. You can move, or remove it, as needed.
            this.gainTableAdapter.Fill(this.babyFootDataSet1.gain);
            // TODO: This line of code loads data into the 'babyFootDataSet.joueur' table. You can move, or remove it, as needed.
            this.joueurTableAdapter.Fill(this.babyFootDataSet.joueur);

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(Form1.idJoueur1 == int.Parse(comboBoxJoeur.SelectedValue.ToString()))
                Form1.jeton1 += int.Parse(textBoxNbrJeton.Text);
            else
                Form1.jeton2 += int.Parse(textBoxNbrJeton.Text);
            labelJeton.Text = "J1 : " + Form1.jeton1 + " jetons --- J2: " + Form1.jeton2 + " jetons";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Form1.mise1 = float.Parse(textBoxMise1.Text);

            Form1.mise2 = float.Parse(textBoxMise2.Text);

            if (Form1.idJoueur1 == int.Parse(comboBoxAcheteurJeton.SelectedValue.ToString()))
                Form1.jeton1--;
            else
                Form1.jeton2--;
            //labelJeton.Text = "J1 : " + Form1.jeton1 + " jetons --- J2: " + Form1.jeton2 + " jetons";
            labelJeton.Text = "";
            this.Hide();
            Form1.nombreBalle = nombreBalle;
            form1.ShowDialog();
            dataGridViewGain.Refresh();
        }

        private void ButtonJouer_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1.nombreBalle = nombreBalle;
            form1.ShowDialog();

            if (Form1.idJoueur1 == int.Parse(comboBoxAcheteurJeton.SelectedValue.ToString()))
                Form1.jeton1--;
            else
                Form1.jeton2--;
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void LabelArgent_Click(object sender, EventArgs e)
        {

        }

        private void LabelStatistique_Click(object sender, EventArgs e)
        {

        }

        private void Actualiser_Click(object sender, EventArgs e)
        {
            decimal[] argent = getRestArgent(Form1.idJoueur1, Form1.idJoueur2);
            labelArgent.Text = "j1: "+argent[0]+ "Ar ----- j2: " + argent[1] + "Ar";
        }
    }
}
