using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProjectForCore
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var model = new TestModel()
            {
                Age = 25,
                Name = "Ana",
            };
            string formula = "";
            formula = ".Age>25"; RunConsoleWithResultBoolean(formula, model, "False");
            formula = ".Age>=25"; RunConsoleWithResultBoolean(formula, model, "True");
            formula = ".Age<25"; RunConsoleWithResultBoolean(formula, model, "False");
            formula = ".Age<=25"; RunConsoleWithResultBoolean(formula, model, "True");
            formula = ".Age+78<=25"; RunConsoleWithResultBoolean(formula, model, "False");
            formula = ".Age+6-8<=25"; RunConsoleWithResultBoolean(formula, model, "True");
            formula = ".Name=Ana & .Age>30"; RunConsoleWithResultBoolean(formula, model, "False");
            formula = ".Name=Ana & .Age>10"; RunConsoleWithResultBoolean(formula, model, "True");

            formula = ".Age+10"; RunConsoleWithResultNumber(formula, model, "35");
            formula = ".Age+10*20"; RunConsoleWithResultNumber(formula, model, "225");
            formula = "(.Age+10)*20"; RunConsoleWithResultNumber(formula, model, "700");
        }
        static void RunConsoleWithResultBoolean(string formula, TestModel model, string shouldBe)
        {
            var formulaProcessor = new For.ExpressionCompareGenerateEngine.FormulaProcess();
            var expressionProcessor = new For.ExpressionCompareGenerateEngine.ExpressionProcess();
            Assert.AreEqual(expressionProcessor
                .GenerateFunc<TestModel, bool>(formulaProcessor.SeparateFormula(formula)).Invoke(model).ToString(), shouldBe);
        }
        static void RunConsoleWithResultNumber(string formula, TestModel model, string shouldBe)
        {
            var formulaProcessor = new For.ExpressionCompareGenerateEngine.FormulaProcess();
            var expressionProcessor = new For.ExpressionCompareGenerateEngine.ExpressionProcess();
            Assert.AreEqual(expressionProcessor
                .GenerateFunc<TestModel, int>(formulaProcessor.SeparateFormula(formula)).Invoke(model).ToString(), shouldBe);
        }
    }
}
