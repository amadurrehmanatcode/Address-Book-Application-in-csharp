using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity.Validation;
using System.Data.Entity;


namespace AddressBook
{
    public partial class Contacts : Form
    {
        public Contacts()
        {
            InitializeComponent();
        }
        private AddressExample.AddressBookEntities DbContext = null;
        private void RefreshContacts()
        {
            if (DbContext != null)
            {
                DbContext.Dispose();
            }
            DbContext = new AddressExample.AddressBookEntities();
            DbContext.Addresses
           .OrderBy(entry => entry.LastName)
           .ThenBy(entry => entry.FirstName)
           .Load();
            addressBindingSource.DataSource = DbContext.Addresses.Local;
            addressBindingSource.MoveFirst();
            findTextBox.Clear();
        }
        private void Contacts_Load(object sender, EventArgs e)
        {
            RefreshContacts();
        }
        private void AddressBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Validate();
            addressBindingSource.EndEdit();
            try
            {
                DbContext.SaveChanges();
            }
            catch (DbEntityValidationException)
            {
                MessageBox.Show("Columns cannot be empty",
                    "Entity Validation Exception");
            }
       
        }
        private void FindButton_Click(object sender, EventArgs e)
        {
            var lastNameQuery =
            from address in DbContext.Addresses
            where address.LastName.StartsWith(findTextBox.Text)
            orderby address.LastName, address.FirstName
            select address;
            addressBindingSource.DataSource = lastNameQuery.ToList();
            addressBindingSource.MoveFirst();
            bindingNavigatorAddNewItem.Enabled = false;
            bindingNavigatorDeleteItem.Enabled = false;
        }
        private void BrowseAllButton_Click(object sender, EventArgs e)
        {
            bindingNavigatorAddNewItem.Enabled = true;
            bindingNavigatorDeleteItem.Enabled = true;
            RefreshContacts();
        }
    }
}
