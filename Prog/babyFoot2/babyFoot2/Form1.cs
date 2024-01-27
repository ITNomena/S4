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
    public partial class Form1 : Form
    {
        //a propos du match
        int limitScore=2;
        public static int nombreBalle=1;
        public static float coutJeton=100;
        Gagne gagne = new Gagne();

        //a propos de la balle
        Boolean passTir = false;
        int balleX=0;
        int balleY=4;
        Boolean ballMove=true;
        int ecart = 50;
        int possedantBallon;
        int vitesseTir=2;
        int vitessepasse = 4;

        // a propos de J1
        int joueurX = 4;
        Boolean moveRightJoueur = false;
        Boolean moveLeftJoueur = false;
        int mainJ1=0;
        String g1 = "g1";
        String d1 = "d1";
        String a1 = "a1";
        int score1 = 0;
        public static int jeton1;
        public static int idJoueur1 = 1;
        public static float mise1 = 0;

        // a propos de J2
        Boolean moveRightJoueur2 = false;
        Boolean moveLeftJoueur2 = false;
        int mainJ2 = 0;
        String g2 = "g2";
        String d2 = "d2";
        String a2 = "a2";
        int score2 = 0;
        public static int jeton2 =0;
        public static int idJoueur2 = 2;
        public static float mise2 = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void PictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void match(object sender, EventArgs e)
        {
            startMatch();
        }

        private void startMatch()
        {
            augmenteScore();
            moveBall();
            String nomPlayer1="";
            if(mainJ1==0)
                nomPlayer1=g1;
            else if(mainJ1 == 1)
                nomPlayer1 = d1; 
            else if (mainJ1 == 2)
                nomPlayer1 = a1;
            movePlayer(nomPlayer1,moveRightJoueur,moveLeftJoueur);
            deplaceBatton(nomPlayer1,batton1);

            String nomPlayer2 = "";
            if (mainJ2 == 0)
                nomPlayer2 = g2;
            else if (mainJ2 == 1)
                nomPlayer2 = d2;                                                                                                                                                                                                                                                      
            else if (mainJ2 == 2)
                nomPlayer2 = a2;
            movePlayer(nomPlayer2,moveRightJoueur2,moveLeftJoueur2);
            deplaceBatton(nomPlayer2,batton2);

            //affichage mise et jeton
            labelMise.Text = "mise j1 : "+mise1+ "Ar ---- mise j2 : " + mise2 + "Ar";

            //insertion des gains
            testIfScoreLimit();

            //test oe lany jeton ve
            //lanyJeton();

            //pass et tir
            if (passTir == true)
                moveBall2();

            //reduit la vitesse de la balle
            if (balleX > 10)
                balleX = 8;
        }

        //game over
        private void lanyJeton()
        {
            if (nombreBalle == 0 || Form1.jeton1 < 0 || Form1.jeton2 < 0)
            {
                //this.matchTimer.Stop();
                this.Hide();
                this.Owner.Show();
            }

        }

        //teste si le score limite est atteint
        public void testIfScoreLimit()
        {
            if (limitScore <= score1)
            {
                score1 = 0;
                score2 = 0;
                mise1 = mise2 - coutJeton;
                mise2 = -mise2;
                insertGain(idJoueur1);

                this.Hide();
                this.Owner.Show();
                Gagne.message = "J1 a gagne "+mise1+" et J2 a perdu "+(-mise2)+" Ar";
                gagne.change_message();
                gagne.ShowDialog();
                
            }
            if (limitScore <= score2)
            {
                score2 = 0;
                score1 = 0;
                mise2 = mise1 - coutJeton;
                mise1 = -mise1;
                insertGain(idJoueur2);
                
                this.Hide();
                this.Owner.Show();
                Gagne.message = "J2 a gagne " + mise2 + " et J1 a perdu " + (-mise1) + " Ar";
                gagne.ShowDialog();
            }
        }

        //inserer dans la base gain si on ganne
        private void insertGain(int id)
        {
            Connexion con = new Connexion();
            SqlConnection connection = Connexion.connexionMysql();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "insert into gain(idJ1,gain1,idJ2,gain2) values("+idJoueur1+","+mise1+","+idJoueur2+","+mise2+");";

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        //test si les scores sont maty
        private void augmenteScore()
        {
            if (balle.Bounds.IntersectsWith(but1.Bounds))
            {
                giveBallToPlayer(g1);
                score2++;
                nombreBalle--;
            }
            if (balle.Bounds.IntersectsWith(but2.Bounds))
            {
                giveBallToPlayer(g2);
                score1++;
                nombreBalle--;
            }

            scoreLabel.Text = "J1 : " + score1 + " - " + score2 + " : J2";
        }

        //donnez la balle au gardient
        private void giveBallToPlayer(String nomPlayer)
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (String)x.Tag == nomPlayer)
                { 
                   balle.Left = x.Left;
                    if (nomPlayer == g1)
                        balle.Top = x.Top + ecart;
                    else
                        balle.Top = x.Top - ecart;
                }
            }
        }

        //deplace la balle
        private void moveBall()
        {
            if (ballMove == true)
            {
                balle.Left += balleX;
                balle.Top += balleY;
                if (balle.Left < 0 || balle.Left > 610)
                {
                    balleX = -balleX;
                }

                if (balle.Top < 0 || balle.Top > 600)
                {
                    balleY = -balleY;
                }

                foreach (Control x in this.Controls)
                {
                    if (x is PictureBox && ((String)x.Tag == g1 || (String)x.Tag == d1 || (String)x.Tag == a1 || (String)x.Tag == g2 || (String)x.Tag == d2 || (String)x.Tag == a2) && passTir==false)
                    {
                        if (x.Bounds.IntersectsWith(balle.Bounds))
                        {
                            balle.Left = x.Left;
                            if ((String)x.Tag == g1 || (String)x.Tag == d1 || (String)x.Tag == a1)
                            {
                                balle.Top = x.Top + ecart;
                                balleX = Math.Abs(balleX);
                                balleY = Math.Abs(balleY);
                                possedantBallon = 1;
                            }
                            else
                            {
                                balle.Top = x.Top - ecart;
                                balleX = -Math.Abs(balleX);
                                balleY = -Math.Abs(balleY);
                                possedantBallon = 2;
                            }
                            ballMove = false;
                            
                        }
                    }
                }
            }
        }

        //deplace la balle 2
        private void moveBall2()
        {
                foreach (Control x in this.Controls)
                {
                    if (x is PictureBox && ((String)x.Tag == g1 || (String)x.Tag == d1 || (String)x.Tag == a1 || (String)x.Tag == g2 || (String)x.Tag == d2 || (String)x.Tag == a2))
                    {
                        if (x.Bounds.IntersectsWith(balle.Bounds))
                        {
                            balleY = balleX * vitesseTir;
                            balleX = 0;
                            passTir = false;
                            ballMove = true;
                        }
                    }
                }
        }

        //deplace les joueurs
        private void movePlayer(String nomPlayer,Boolean moveRightJoueur, Boolean moveLeftJoueur)
        {

            if (moveRightJoueur == true && determineSiJoueurAuBordDroite(nomPlayer) ==false)
            {
                foreach (Control x in this.Controls)
                {
                    if (x is PictureBox && (String)x.Tag == nomPlayer)
                    {
                        int identifiant = 0;
                        if (balle.Left == x.Left)
                            identifiant=1;
                        x.Left += joueurX;
                        if(identifiant == 1 && ballMove==false)
                          balle.Left = x.Left;
                    }
                }
            }

            if (moveLeftJoueur == true && determineSiJoueurAuBordGauche(nomPlayer) ==false)
            {
                foreach (Control x in this.Controls)
                {
                    if (x is PictureBox && (String)x.Tag == nomPlayer)
                    {
                        int identifiant = 0;
                        if (balle.Left == x.Left)
                            identifiant = 1;
                        x.Left -= joueurX;
                        if (identifiant == 1 && ballMove == false)
                            balle.Left = x.Left;
                    }
                }
            }
        }

        //determine Si Joueur Au Bord Droite
        private Boolean determineSiJoueurAuBordDroite(String nomPlayer)
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (String)x.Tag == nomPlayer)
                {
                    if (x.Left > 430)
                        return true;
                }
            }
            return false;
        }


        //determine Si Joueur Au Bord Gauche
        private Boolean determineSiJoueurAuBordGauche(String nomPlayer)
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (String)x.Tag == nomPlayer)
                {
                    if (x.Left < 0)
                        return true;
                }
            }
            return false;
        }

        //deplace le batton
        private void deplaceBatton(String nomPlayer,PictureBox batton)
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (String)x.Tag == nomPlayer)
                {
                    batton.Top = x.Top;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'babyFootDataSet.joueur' table. You can move, or remove it, as needed.
            this.joueurTableAdapter.Fill(this.babyFootDataSet.joueur);

        }

        private void PictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            //passer puis tirer
            if (e.KeyCode == Keys.Space)
            {
                balleX = balleY;
                if(possedantBallon == 1)
                    balleY = -2;
                else
                    balleY = 2;
                passTir = true;
                ballMove = true;
            }

            //tirer
            if (e.KeyCode == Keys.C || e.KeyCode == Keys.Shift)
            {
                balleY = balleY*vitesseTir;
                ballMove = true;
            }

            // move the ball p1
            if (e.KeyCode == Keys.X && possedantBallon == 1)
            {
                if(balleY>vitessepasse)
                    balleY = balleY / vitesseTir;
                ballMove = true;
            }

            // move the ball p2
            if (e.KeyCode == Keys.Up && possedantBallon==2)
            {
                balleY = balleY / vitesseTir;
                ballMove = true;
            }

            // move the player1 to the right
            if (e.KeyCode == Keys.D)
            {
                moveRightJoueur = true;
            }

            // move the player1 to the left
            if (e.KeyCode == Keys.A)
            {
                moveLeftJoueur = true;
            }

            // move the player2 to the right
            if (e.KeyCode == Keys.Right)
            {
                moveRightJoueur2 = true;
            }

            // move the player2 to the left
            if (e.KeyCode == Keys.Left)
            {
                moveLeftJoueur2 = true;
            }

            if (e.KeyCode == Keys.S )
            {
                mainJ1 = (mainJ1+1)%3;
            }

            if (e.KeyCode == Keys.Down)
            {
                mainJ2 = (mainJ2+1) % 3;
            }
        }

        private void keyUp(object sender, KeyEventArgs e)
        {
            // disable move the player1 to the right
            if (e.KeyCode == Keys.D)
            {
                moveRightJoueur = false;
            }

            // disable move the player1 to the left
            if (e.KeyCode == Keys.A)
            {
                moveLeftJoueur = false;
            }

            // disable move the player2 to the right
            if (e.KeyCode == Keys.Right)
            {
                moveRightJoueur2 = false;
            }

            // disable move the player2 to the left
            if (e.KeyCode == Keys.Left)
            {
                moveLeftJoueur2 = false;
            }
        }

        private void keyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void PictureBox2_Click_1(object sender, EventArgs e)
        {

        }

        private void PictureBox17_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox18_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox13_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox15_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox14_Click(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void FillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.joueurTableAdapter.FillBy(this.babyFootDataSet.joueur);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void GroupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void Label5_Click(object sender, EventArgs e)
        {

        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void FillByToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            balle.Focus();
        }

        private void GroupBox3_Enter(object sender, EventArgs e)
        {
            //groupBox3.Focus();
        }

        private void ButtonMenuPrincipal_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        private void PictureBox16_Click(object sender, EventArgs e)
        {
            
        }

        
    }
}
