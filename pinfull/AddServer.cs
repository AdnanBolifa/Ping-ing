using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace pinfull {
    public partial class AddServer : Form {
        public AddServer() {
            InitializeComponent();
        }

        private void AddServer_Load(object sender, EventArgs e) {

        }
        public ServerInfo Server { get; private set; }

        private void button1_Click(object sender, EventArgs e) {
            // Validate input (you can add more validation if needed)
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                string.IsNullOrWhiteSpace(ipTextBox.Text)) {
                MessageBox.Show("Name and IP are required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create a new ServerInfo object with the provided details
            Server = new ServerInfo {
                Name = nameTextBox.Text,
                Ip = ipTextBox.Text,
                Description = descriptionTextBox.Text,
                Location = locationTextBox.Text
            };

            // Set the DialogResult to OK to indicate that the user clicked OK
            DialogResult = DialogResult.OK;

            // Close the form
            Close();
        }
    }
}
