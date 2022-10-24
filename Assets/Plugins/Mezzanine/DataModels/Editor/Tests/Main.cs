using NUnit.Framework;
using UnityEngine;
using System;
using Mz.Models;
using Mz.Serializers;

namespace Plugins.Mezzanine.DataModels.Editor.Tests
{
    public class Employee
    {
        public Employee()
        {
            Reviews = new[] { 3, 4, 3, 5, 2 };
        }
        
        public Employee(string firstName, string lastName) : this()
        {
            FirstName = firstName;
            LastName = lastName;
            Age = 0;
        }
        
        public int[] Reviews { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; set; }
        public int Age { get; private set; }
    }

    public class Department
    {
        public Department() { }

        public Department(string title, Employee manager)
        {
            Title = title;
            Manager = manager;
        }

        public string Title { get; private set; }
        public Employee Manager { get; private set; }
    }

    public class Organization
    {
        public Organization() { }

        public Organization(
            string name,
            Department developmentDepartment,
            ModelCollection<Department> departments
        )
        {
            Name = name;
            DevelopmentDepartment = developmentDepartment;
            Departments = departments;
        }

        public string Name { get; private set; }
        public Department DevelopmentDepartment { get; private set; }
        public ModelCollection<Department> Departments { get; private set; }
    }

    public class Game
    {
        public bool IsMusic { get; set; } = false;
    }

    [TestFixture]
    public class Test
    {
        [Test]
        public void Simple()
        {
            var employee1 = new Employee("Stan", "The Man");
            var model = Model.Create(employee1);
            
            Debug.Log($"Initial model values, key: {model.Key}, name: {model.Data.FirstName} {model.Data.LastName}");

            model.Changed += modelChangedArgs =>
            {
                Debug.Log($"Model changes: {modelChangedArgs.Changes.Count}, name: {modelChangedArgs.Model.Data.FirstName} {modelChangedArgs.Model.Data.LastName}, age: {modelChangedArgs.Model.Data.Age}, change 0: {modelChangedArgs.Changes[0].ValueCurrent}");
            };

            var modelParallel = model.Parallel;
            modelParallel.Set(data => data.FirstName, "John");
            modelParallel.Set(data => data.LastName, "Smith");
            modelParallel.Build();
            
            Assert.AreEqual("John", model.Data.FirstName);
            Assert.AreEqual("Smith", model.Data.LastName);

            modelParallel.Set(data => data.FirstName, "Dude");
            modelParallel.Set(data => data.Age, 2);
            modelParallel.Build();

            Debug.Log($"Age: {model.Data.Age}");

            var cloneData = model.CloneData();
            Debug.Log($"Age: {cloneData.Age}");
        }
        
        [Test]
        public void Instantiation()
        {
            var model1 = new Model<Employee>();
            var model2 = Model.Create<Employee>();

            model1.Set(data => data.FirstName, "Keanu").Set(data => data.LastName, "Reeves");
            Assert.AreEqual("Keanu", model1.Data.FirstName);
            
            model2.Set(data => data.FirstName, "The").Set(data => data.LastName, "One");
            Assert.AreEqual("One", model2.Data.LastName);
        }
        
        [Test]
        public void ModelTest()
        {
            var employee_1 = new Employee("Stan", "The Man");
            var employee_2 = new Employee("Jane", "The Brain");
            var department = new Department("Central Processing", employee_1);
            var departments = new ModelCollection<Department>();
            var organization = new Organization("Big Brother", department, departments);
            var model = new Model<Organization>(organization);

            model.Changed += args =>
            {
                Debug.Log($"Model changes: {args.Changes.Count}");
            };

            Debug.Log($"Department manager: {model.Data.DevelopmentDepartment.Manager.FirstName}");

            model.Set(data => data.DevelopmentDepartment.Manager, employee_2);
            Debug.Log($"Done waiting 1");
            Debug.Log($"Department manager: {model.Data.DevelopmentDepartment.Manager.FirstName}");

            model.Set(data => data.DevelopmentDepartment.Title, "Bureau of Affairs");
            Debug.Log($"Department title: {model.Data.DevelopmentDepartment.Title}");

            var employee_3 = new Employee("Vernon", "DeLong");
            var department_2 = new Department("Central Processing", employee_1);
            var organization_2 = new Organization("Big Brother", department_2, departments);
            var model_2 = new Model<Organization>(organization_2);
            model.Initialize(organization_2);

            Debug.Log($"Manager: {model.Data.DevelopmentDepartment.Manager.FirstName}");

            var modelClone = model.Set(
                data => data.DevelopmentDepartment.Title, 
                "Bureau of Bureaucracy"
            ).Data;
            
            Debug.Log($"Done waiting");
            var modelCloneClone = model.CloneData();
            Debug.Log($"Department title: {modelCloneClone.DevelopmentDepartment.Title}");
        }

        [Test]
        public void Collection()
        {
            var employee_1 = new Employee("Stan", "The Man");
            var employee_2 = new Employee("Jane", "The Brain");
            var department = new Department("Central Processing", employee_1);
            var departments = new ModelCollection<Department>();
            var organization = new Organization("Big Brother", department, departments);

            var modelDepartment = organization.Departments.Add(department);
            var departmentNew = new Department("Maintainence", employee_1);
            organization.Departments.Add(departmentNew);

            var modelOrganization = new Model<Organization>(organization);
            modelOrganization.Changed += (args) =>
            {
                Debug.Log($"Properties were set: {args.Changes.Count}");
            };

            var orgImmutable = modelOrganization.CloneData();
            orgImmutable = modelOrganization.Set(
                org => org.DevelopmentDepartment.Manager.FirstName, 
                "Barney", 
                true
            ).Data;

            Debug.Log(
                $"org.DevelopmentDepartment.Manager.FirstName: {orgImmutable.DevelopmentDepartment.Manager.FirstName}");
            Debug.Log($"department count: {orgImmutable.Departments.Count}");

            Data.SerializeToString(orgImmutable, out var serialized, DataFormat.Json, true);
            Console.Write($"{serialized}");
        }

        [Test]
        public void Serialize()
        {
            var employee_1 = new Employee("Stan", "The Man");
            var employee_2 = new Employee("Jane", "The Brain");
            var department = new Department("Central Processing", employee_1);
            var departments = new ModelCollection<Department>();
            var organization = new Organization("Big Brother", department, departments);

            var modelDepartment = organization.Departments.Add(department);
            var departmentNew = new Department("Maintainence", employee_1);
            organization.Departments.Add(departmentNew);
            var modelOrganization = new Model<Organization>(organization, "organization");
            
            Data.DataSubDirectory = "Mz_Data_Tests"; // This directory will not be automatically created.
            var isSuccess = modelOrganization.Save();
        }

        [Test]
        public void Derialize()
        {
            var fileName = "organization";
            Model.IsUnity = true;
            var model = Model.LoadResource<Organization>(fileName, "Mz_Data_Tests");
            TestContext.WriteLine($"Name: {model.Data.Name}");
        }
        
        [Test]
        public void GameTest()
        {
            var game = Model.Create<Game>();
            game.Set(data => data.IsMusic, true);
            Debug.Log($"game.Current.IsMusic: {game.Data.IsMusic}");
        }

        public class EmployeeModel : Model<Employee>
        {
            public EmployeeModel() {}
            public EmployeeModel(Employee employee, object key = null) : base(employee, key) {}

            public static EmployeeModel Create(Employee employee)
            {
                return new EmployeeModel(employee, $"{employee.LastName}_{employee.FirstName}");
            }

            public void SetReview(int index, int value)
            {
                var reviewsNew = Data.Reviews;
                reviewsNew[index] = value;
                Set(data => data.Reviews, reviewsNew);
            }
        }
        
        [Test]
        public void Arrays()
        {
            var model3 = Model.Create<EmployeeModel, Employee>();
            Debug.Log($"Review 0: {model3.Data.Reviews[0]}");

            model3.Changed += (args) =>
            {
                Debug.Log($"Model changes: {args.Changes.Count}, change: {args.Changes[0].ValueCurrent}");
            };
            
            var reviewsNew = model3.Data.Reviews;
            reviewsNew[0] = -4;
            model3.Set(data => data.Reviews, reviewsNew, true);
            Debug.Log($"Employee 3, review 0: {model3.Data.Reviews[0]}");
            Assert.AreEqual(model3.Data.Reviews[0], -4);
            
            model3.SetReview(0, -5);
            Debug.Log($"Employee 3, review 0: {model3.Data.Reviews[0]}");
            Assert.AreEqual(model3.Data.Reviews[0], -5);
        }

        // This only applies if REFLECTION_EMIT is defined before Mz.Models is loaded
        // [Test]
        // public void PropertySetDelegate()
        // {
        //     var employee = new Employee();
        //     
        //     Debug.Log($"type name: {employee.GetType().Name}, property name: {nameof(employee.FirstName)}");
        //     var setter = Mz.TypeTools.Types.GetPropertySetter(employee.GetType(), nameof(employee.FirstName));
        //
        //     setter?.Invoke(employee, "Daniel");
        //     
        //     Debug.Log($"employee name: {employee.FirstName}");
        // }
    }
}