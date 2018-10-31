using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Capstone
{
    [TestClass]
    public class UnitTest1
    {
        VendingMachine vm;

        [TestInitialize]
        public void Initialize()
        {
            vm = new VendingMachine();

        }


        [TestMethod]
        public void MakeChangeTest()
        {
            Change change = new Change(10.00, 9.35);

            Assert.AreEqual(2, change.NumOfQuarters);
            Assert.AreEqual(1 , change.NumOfDimes);
            Assert.AreEqual(1, change.NumOfNickels);

            Change change2 = new Change(5.00, 4.10);

            Assert.AreEqual(3, change2.NumOfQuarters);
            Assert.AreEqual(1, change2.NumOfDimes);
            Assert.AreEqual(1, change2.NumOfNickels);

        }

        [TestMethod]
        public void AddMoneyTest()
        {
            Assert.AreEqual(10, vm.AddMoney(10));
            Assert.AreEqual(11, vm.AddMoney(1));
            Assert.AreEqual(31, vm.AddMoney(20));

        }

        [TestMethod]
        public void MakeSoundTest()
        {
            Candy candy = new Candy("B1", "Moonpie", 1.80);
            Gum gum = new Gum("D4", "Triplemint", 0.75);
            Chip chip = new Chip("A2", "Stackers", 1.45);
            Drink drink = new Drink("C1", "Cola", 1.25);

            Assert.AreEqual("Munch Munch, Yum!", candy.MakeSound());
            Assert.AreEqual("Glug Glug, Yum!", drink.MakeSound());
            Assert.AreEqual("Chew Chew, Yum!", gum.MakeSound());
            Assert.AreEqual("Crunch Crunch, Yum!", chip.MakeSound());

        }
    }
}
