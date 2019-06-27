using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsAppTest.BusinessLogic;

namespace WindowsFormsAppTest
{
    public class App
    {
        /// <summary>
        /// Connection database
        /// </summary>
        TestDBEntities db;
        /// <summary>
        /// Number selected depatment
        /// </summary>
        public Guid number;

        /// <summary>
        /// First load data
        /// </summary>
        /// <returns>All Tree depatments</returns>
        public TreeNode LoadDataTreeDepartment()
        {
            db = new TestDBEntities();
            List<Department> dep = db.Departments.ToList();

            Department d = dep.SingleOrDefault(i => i.ParentDepartmentID is null);
            TreeNode root = new TreeNode()
            {
                Name = d.ID.ToString(),
                Tag = d.ID,
                Text = d.Name,
            };
            FindNodeDepartment(root, dep);
            return root;
        }

        /// <summary>
        /// Recursive search node 
        /// </summary>
        /// <param name="node">Top node</param>
        /// <param name="dept">Organization all departments</param>
        private void FindNodeDepartment(TreeNode node, List<Department> dept)
        {
            List<Department> list = dept.Where(i => (i.ParentDepartmentID).ToString() == node.Tag.ToString()).ToList();
            if (list != null)
            {
                foreach (Department dep in list)
                {
                    TreeNode child = new TreeNode();
                    child.Name = dep.ID.ToString();
                    child.Tag = dep.ID;
                    child.Text = dep.Name;
                    FindNodeDepartment(child, dept);
                    node.Nodes.Add(child);
                }
            }
        }

        /// <summary>
        /// Find all empoyees in department
        /// </summary>
        /// <param name="e"> Selected TreeNode</param>
        /// <returns></returns>
        public List<Empoyee> SelectedNodeDepartment(TreeNode e)
        {
            using (db = new TestDBEntities())
            {
                number = db.Empoyees.First(i => i.Department.ID.ToString() == e.Tag.ToString()).DepartmentID;
                return db.Empoyees.Where(i => i.Department.ID.ToString() == number.ToString()).ToList();
            }
        }

        /// <summary>
        /// Load data selected empoyee
        /// </summary>
        /// <param name="numberID"> id selected empoyee </param>
        /// <returns></returns>
        public Empoyee LoadSelectedEmpoyee(decimal numberID)
        {
            using (db = new TestDBEntities())
            {
                return db.Empoyees.Where(x => x.ID == numberID).FirstOrDefault();
            }
        }

        /// <summary>
        /// Update or save empoyee
        /// </summary>
        /// <param name="em">Empoyee</param>
        /// <param name="Id">Number department</param>
        /// <returns></returns>
        public List<Empoyee> UpdateAndSaveEmpoyee(Empoyee em, decimal Id)
        {
            Empoyee emLocal = new Empoyee();
            using (db = new TestDBEntities())
            {
                if (Id != 0)
                {
                    emLocal = db.Empoyees.Where(x => x.ID == Id).FirstOrDefault();
                }
                emLocal.ID = Id;
                emLocal.DepartmentID = number;
                emLocal.FirstName = em.FirstName;
                emLocal.SurName = em.SurName;
                emLocal.DateOfBirth = em.DateOfBirth;
                emLocal.Position = em.Position;
                emLocal.DocNumber = em.DocNumber;
                emLocal.DocSeries = em.DocSeries;
                emLocal.Patronymic = em.Patronymic;

                if (!Validator.MainValidator(emLocal))
                    {
                    MessageBox.Show(Validator.SendErrorMessage());
                    return db.Empoyees.Where(i => i.Department.ID.ToString() == number.ToString()).ToList();
                }


                if (emLocal.ID != 0)
                {
                    db.Entry(emLocal).State = EntityState.Modified;
                    db.SaveChanges();
                    MessageBox.Show("Обновлен");
                }
                else
                {
                    db.Empoyees.Add(emLocal);
                    db.SaveChanges();
                    MessageBox.Show("Добавлен");
                }
                return db.Empoyees.Where(i => i.Department.ID.ToString() == number.ToString()).ToList();
            }
        }
    }
}
