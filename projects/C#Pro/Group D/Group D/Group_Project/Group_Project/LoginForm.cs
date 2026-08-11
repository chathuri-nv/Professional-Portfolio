using System;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Group_Project
{
    public partial class LoginForm : Form
    {
       
        DataClasses1DataContext db = new DataClasses1DataContext();

        public LoginForm()
        {
            InitializeComponent();

            
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        
        private void Login_Click(object sender, EventArgs e)
        {
            try
            {
               
                var user = db.Users.FirstOrDefault(u => u.Username == textBox1.Text 
                && u.Password == textBox3.Text);

                if (user != null)
                {
                    DashboardForm dash = new DashboardForm();
                    dash.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password!", "Login Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox3.Text.Trim();

          
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both Username and Password.");
                return;
            }

            
            Regex nameRegex = new Regex("^[A-Z][a-zA-Z]*$");
            if (!nameRegex.IsMatch(username))
            {
                MessageBox.Show("Username must start with a Capital letter and contain only letters.",
                    
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
            Regex passwordRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&-]).{8,}$");
            if (!passwordRegex.IsMatch(password))
            {
                MessageBox.Show("Password is too weak!\n\nMust have:\n- Minimum 8 " +
                    "characters\n- One Uppercase letter\n- One Lowercase letter\n- One Number\n- One Special character", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
               
                bool userExists = db.Users.Any(u => u.Username == username);

                if (userExists)
                {
                    MessageBox.Show("This username already exists. Please choose another one.");
                }
                else
                {
                    
                    User newUser = new User
                    {
                        Username = username,
                        Password = password
                    };

                    db.Users.InsertOnSubmit(newUser);
                    db.SubmitChanges();

                    MessageBox.Show("Account created successfully! Now you can log in.", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                  
                    textBox1.Clear();
                    textBox3.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Signup Error: " + ex.Message);
            }
        }

       
        private void button2_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        
    }
}