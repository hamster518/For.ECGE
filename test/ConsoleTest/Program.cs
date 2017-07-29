using System;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = new TestModel()
            {
                Age = 25,
                Name = "Ana",
            };
            string formula = "";
            formula = ".Age>25"; RunConsoleWithResultBoolean(formula, model,formula + "__False");
            formula = ".Age>=25"; RunConsoleWithResultBoolean(formula, model, formula + "__True");
            formula = ".Age<25"; RunConsoleWithResultBoolean(formula, model, formula + "__False");
            formula = ".Age<=25"; RunConsoleWithResultBoolean(formula, model, formula + "__True");
            formula = ".Age+78<=25"; RunConsoleWithResultBoolean(formula, model, formula + "__False");
            formula = ".Age+6-8<=25"; RunConsoleWithResultBoolean(formula, model, formula + "__True");
            formula = ".Name=Ana & .Age>30"; RunConsoleWithResultBoolean(formula, model, formula + "__False");
            formula = ".Name=Ana & .Age>10"; RunConsoleWithResultBoolean(formula, model, formula + "__True");

            formula = ".Age+10"; RunConsoleWithResultNumber(formula, model, formula + "__35");
            formula = ".Age+10*20"; RunConsoleWithResultNumber(formula, model, formula + "__225");
            formula = "(.Age+10)*20"; RunConsoleWithResultNumber(formula, model, formula + "__700");
            Console.ReadLine();
        }

        static void RunConsoleWithResultBoolean(string formula, TestModel model, string shouldBe)
        {
            var formulaProcessor = new For.ExpressionCompareGenerateEngine.FormulaProcess();
            var expressionProcessor = new For.ExpressionCompareGenerateEngine.ExpressionProcess();
            Console.WriteLine(expressionProcessor
                .GenerateFunc<TestModel, bool>(formulaProcessor.SeparateFormula(formula)).Invoke(model) +"__" + shouldBe);
        }
        static void RunConsoleWithResultNumber(string formula, TestModel model, string shouldBe)
        {
            var formulaProcessor = new For.ExpressionCompareGenerateEngine.FormulaProcess();
            var expressionProcessor = new For.ExpressionCompareGenerateEngine.ExpressionProcess();
            Console.WriteLine(expressionProcessor
                                  .GenerateFunc<TestModel, int>(formulaProcessor.SeparateFormula(formula)).Invoke(model) + "__" + shouldBe);
        }
    }
}