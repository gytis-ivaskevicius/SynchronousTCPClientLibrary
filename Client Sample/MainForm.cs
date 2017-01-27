using SynchronousTCPClientLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_Sample
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();


            TCPClient.onConnected += connected;
            TCPClient.onDisconnect += disconnected;
            TCPClient.onDataReceived += received;
        }

        private void send_btn_Click(object sender, EventArgs e)
        {
            if (send_cbox.Checked)
            {
                TCPClient.sendln(send_txt.Text);
                console_txt.AppendText("Client: " + send_txt.Text + Environment.NewLine + Environment.NewLine);
            }
            else
            {
                TCPClient.send(send_txt.Text);
                console_txt.AppendText("Client: " + send_txt.Text + Environment.NewLine);
            }
        }

        private void appendData(string data)
        {
            Func<int> l = delegate ()
            {
                console_txt.AppendText(data + Environment.NewLine);
                return 0;
            };
            try { Invoke(l); } catch (Exception) { }
        }

        private void updateUIOnConnect(Boolean connected)
        {
            Func<int> l = delegate ()
            {
                if (connected)
                {
                    connect_btn.Text = "Disconnect";
                    Connection_lbl.Text = "Connected";
                    Connection_lbl.ForeColor = Color.LawnGreen;
                    ip_txt.Enabled = false;
                    port_txt.Enabled = false;

                    send_btn.Enabled = true;
                    send_txt.Enabled = true;
                }
                else
                {
                    connect_btn.Text = "Connect";
                    Connection_lbl.Text = "Disconnected";
                    Connection_lbl.ForeColor = Color.Red;
                    ip_txt.Enabled = true;
                    port_txt.Enabled = true;

                    send_btn.Enabled = false;
                    send_txt.Enabled = false;
                }
                return 0;
            };
            try { Invoke(l); } catch (Exception) { }
        }
        private void connect_btn_Click(object sender, EventArgs e)
        {
            if (connect_btn.Text.Equals("Disconnect"))
                TCPClient.stopClient();
            else
                TCPClient.connectFromNewThread(ip_txt.Text, port_txt.Text);
        }


        private void received(string message)
        {
            appendData("Server: " + message);
        }

        private void connected(Socket socket)
        {
            updateUIOnConnect(true);
            appendData("Connected to: " + socket.LocalEndPoint);
        }

        private void disconnected(Socket socket)
        {
            updateUIOnConnect(false);
            appendData("Disconnected!!!");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            TCPClient.stopClient();
        }
    }
}
