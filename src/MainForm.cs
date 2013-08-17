using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Generator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void PasswordChanged(object sender, EventArgs e)
        {
            txtSalt.Text = GenerateSalt();
            txtHash.Text = GenerateHash(txtSalt.Text, txtPassword.Text);
        }

        /// <summary>
        /// Generates a random salt value
        /// </summary>
        /// <returns>A base64 encoded salt string</returns>
        private string GenerateSalt()
        {
            byte[] data = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(data);
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// Generates a password hash from a <paramref name="password"/> and <paramref name="salt"/> value.
        /// </summary>
        /// <param name="salt">Salt to use for the hash</param>
        /// <param name="password">Password to be hashed</param>
        /// <returns>A salted base64 encoded password hash</returns>
        private string GenerateHash(string salt, string password)
        {
            var algorithm = new SHA1CryptoServiceProvider();
            var saltBytes = Convert.FromBase64String(salt);
            var passwordBytes = Encoding.Unicode.GetBytes(password);

            var cryptobits = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, cryptobits, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, cryptobits, saltBytes.Length, passwordBytes.Length);

            var hash = algorithm.ComputeHash(cryptobits);
            return Convert.ToBase64String(hash);
        }
    }
}