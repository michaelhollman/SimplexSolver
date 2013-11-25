using RaikesSimplexService.HollmanJagerJohnson;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using RaikesSimplexService.DataModel;

namespace UnitTests
{
    /// <summary>
    ///This is a test class for SolverTest and is intended
    ///to contain all SolverTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SolverTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///Provided test in distributed homework
        ///</summary>
        [TestMethod()]
        public void ExampleSolveTest()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 8, 12 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 24
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 12, 12 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 36
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 2, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 4
            };

            var lc4 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 5
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3, lc4 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 0.2, 0.3 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Minimize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 3, 0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 0.6
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            //Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.IsTrue(ApproxEqual(expected.OptimalValue, actual.OptimalValue));
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        /// <summary>
        ///First Example from LPSimplexRevised2SlidesPerSheet.pptx.pdf
        ///</summary>
        [TestMethod()]
        public void PptRevisedEx1()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 10, 5 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 50
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 6, 6 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 36
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 4.5, 18 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 81
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 9, 7 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 4, 2 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 50
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        /// <summary>
        ///First Example from LPTwoPhaseMethod2SlidesPerSheet.pptx.pdf
        ///</summary>
        [TestMethod()]
        public void PptTwoPhaseEx1()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 1
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 2, -1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 1
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 0, 3 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 2
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 6, 3 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Quality = SolutionQuality.Unbounded,
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            //CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            //Assert.AreEqual(expected.Quality, actual.Quality);
            //Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        /// <summary>
        ///First Example from LPTwoPhaseMethod2SlidesPerSheet.pptx.pdf
        ///</summary>
        [TestMethod()]
        public void PptTwoPhaseEx2()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 1
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 2, -1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 1
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 0, 3 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 2
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 6, 3 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Minimize
            };

            var expected = new Solution()
            {
                Decisions = new double[] { 2.0 / 3.0, 1.0 / 3.0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 5
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            //CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.IsTrue(ApproxEqualArray(expected.Decisions, actual.Decisions));
            //Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.IsTrue(ApproxEqual(expected.OptimalValue, actual.OptimalValue));
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        /// <summary>
        ///Provided test in distributed homework
        ///</summary>
        [TestMethod()]
        public void InClassTwoPhase()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 1
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 2, -1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 1
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 0, 3 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 2
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 6, 3 },
                ConstantTerm = 10
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 1, 0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 16
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            Assert.IsTrue(ApproxEqualArray(expected.Decisions, actual.Decisions));
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        /// <summary>
        ///Third Example from LPSimplexMethod.pptx.pdf
        ///</summary>
        [TestMethod()]
        public void IHaveNoIdeaWhereThisTestCameFromDuringAMergeConflict()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 35
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 2 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 38
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 2, 2 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 50
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 350, 450 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 12, 13 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 10050
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }


        /// <summary>
        ///Third problem from week 4 homework
        ///</summary>
        [TestMethod()]
        public void Week4HWQ3()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[3] { 1, 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 40
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[3] { 2, 1, -1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 10
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[3] { 0, -1, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 10
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[3] { 2, 3, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[] { 10, 10, 20 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 70
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            //CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.IsTrue(ApproxEqualArray(expected.Decisions, actual.Decisions));
            Assert.IsTrue(ApproxEqual(expected.OptimalValue, actual.OptimalValue));
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }


        /// <summary>
        ///First Example from LPSimplexMethod.pptx.pdf
        ///Tests ability of solver to solve a "Minimize Goal" problem
        ///Solver must convert the Minimize goal to a Maximize goal, then can proceed normally
        ///</summary>
        [TestMethod()]
        public void Minimize()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 50, 24 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 2400
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 30, 33 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 2100
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 20
            };

            var lc4 = new LinearConstraint()
            {
                Coefficients = new double[2] { 0, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 5
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3, lc4 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 1, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Minimize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 20, 5 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 25
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            Assert.IsTrue(ApproxEqualArray(expected.Decisions, actual.Decisions));
            Assert.IsTrue(ApproxEqual(expected.OptimalValue, actual.OptimalValue));
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        /// <summary>
        /// #1 from http://www.ms.uky.edu/~rwalker/Class%20Work%20Solutions/class%20work%208%20solutions.pdf
        ///</summary>
        [TestMethod()]
        public void MsUKY1()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 4
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 2, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 5
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 3, 4 },
                ConstantTerm = 10
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 0, 4 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 26
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        /// <summary>
        /// #2 from http://www.ms.uky.edu/~rwalker/Class%20Work%20Solutions/class%20work%208%20solutions.pdf
        ///</summary>
        [TestMethod()]
        public void MsUKY2()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 2 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 6
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 3, 2 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 12
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { -2, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Minimize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 4, 0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = -8
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        /// <summary>
        /// #3 from http://www.ms.uky.edu/~rwalker/Class%20Work%20Solutions/class%20work%208%20solutions.pdf
        ///</summary>
        [TestMethod()]
        public void MsUKY3()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 2 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 4
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 3, 2 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 3
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 2, 5 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Minimize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 4, 0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 8
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        /// <summary>
        ///First Example from LPSimplexMethod.pptx.pdf
        ///Tests ability of solver to solve a "Minimize Goal" problem
        ///Solver must convert the Minimize goal to a Maximize goal, then can proceed normally
        ///</summary>
        [TestMethod()]
        public void Maximize()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 50, 24 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 2400
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 30, 33 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 2100
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 20
            };

            var lc4 = new LinearConstraint()
            {
                Coefficients = new double[2] { 0, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 5
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3, lc4 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 1, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 960.0 / 31.0, 1100.0 / 31.0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                //OptimalValue = 2160.0 / 31.0
                OptimalValue = 66.451612903225808
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            //CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.IsTrue(ApproxEqualArray(expected.Decisions, actual.Decisions));
            //Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.IsTrue(ApproxEqual(expected.OptimalValue, actual.OptimalValue));
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        /// <summary>
        ///First Example from http://optlab.mcmaster.ca/feng/4O03/LP.Degeneracy.pdf
        ///Tests ability of solver to solve a "Degenerate" problem
        ///A Degenerate problem has redundant constraints that can cause extra iterations
        ///Discover by finding if one or more basic variables take a value of 0
        ///</summary>
        [TestMethod()]
        public void Degenerate()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[3] { 1, 1, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 1
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[3] { 0, -1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 0
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2 };

            var goal = new Goal()
            {
                Coefficients = new double[3] { 1, 1, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[3] { 0, 1, 1 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 2
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        /// <summary>
        ///First Example from http://lpsolve.sourceforge.net/5.5/Infeasible.htm
        ///Tests ability of solver to diagnose an infeasible problem
        ///Discover by finding that a RHS value of a constraint variable is < 0
        ///OR
        ///An artificial variable stays basic after phase 1 is optimal
        ///</summary>
        [TestMethod()]
        public void Infeasible()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 6
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 0, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 6
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 11
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 1, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Minimize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 5, 6 },
                Quality = SolutionQuality.Infeasible,
                AlternateSolutionsExist = true,
                OptimalValue = 11
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            //CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            //Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            //Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        /// <summary>
        ///First Example from http://www2.isye.gatech.edu/~spyros/LP/node12.html
        ///Tests ability of solver to diagnose an "unbounded" problem
        ///An unbounded LP is one where we can drive the Objective Value to
        ///positive or negative infinity
        ///Discover by finding when no ratio of RHS to entering variable
        ///coefficient is greater than 0
        ///</summary>
        [TestMethod()]
        public void Unbounded()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, -1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 1
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 2, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 6
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 2, -1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Quality = SolutionQuality.Unbounded,
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            //CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            //Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            //Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        //// <summary>
        ////First Example from http://www.mathstools.com/section/main/infinite_Solution_simpleSample#.Uqai1fmsiSo
        //// Tests the solver's ability to solve a problem with infinite solutions
        //// If any non-basic DECISION variable in the optimal solution has a value of zero in the objective
        //// row then that variable indicates that multiple optimal alternative solutions exist
        ////</summary>
        [TestMethod()]
        public void InfiniteSols()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 3
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 2
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, -1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 1
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 1, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                // Decisions = new double[2] { 2, 0 },
                Decisions = new double[2] { 1.5, .5 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = true,
                OptimalValue = 2
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        ///<summary> 
        ///Here's a super basic test case to see if we can solve super basic test cases!
        ///It's a OneVariableMinimize that should require the use of an artifical variable.
        ///</summary>
        [TestMethod()]
        public void OneVariableMinimize()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[1] { 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 1
            };

            var constraints = new List<LinearConstraint>() { lc1 };

            var goal = new Goal()
            {
                Coefficients = new double[1] { 2 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Minimize
            };

            var expected = new Solution()
            {
                Decisions = new double[1] { 1 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 2
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }


        ///<summary> 
        ///Here's a super basic test case to see if we can solve super basic test cases!
        ///It's a OneVariableMaximize that is one phase.
        ///</summary>
        [TestMethod()]
        public void OneVariableMaximize()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[1] { 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 1
            };

            var constraints = new List<LinearConstraint>() { lc1 };

            var goal = new Goal()
            {
                Coefficients = new double[1] { 2 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[1] { 1 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 2
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        ////<summary> 
        ////Nurse Problem, Maximize all of the constraints! 
        ////</summary>
        [TestMethod()]
        public void NurseProblem()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[21] { 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 5
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[21] { 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 3
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[21] { 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 2
            };

            var lc4 = new LinearConstraint()
            {
                Coefficients = new double[21] { 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 4
            };

            var lc5 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 3
            };

            var lc6 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 2
            };

            var lc7 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 2
            };

            var lc8 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 7
            };

            var lc9 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 8
            };

            var lc10 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 9
            };

            var lc11 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 5
            };

            var lc12 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 7
            };

            var lc13 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 2
            };

            var lc14 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 5
            };

            var lc15 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 9
            };

            var lc16 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 10
            };

            var lc17 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 10
            };

            var lc18 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 7
            };

            var lc19 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 11
            };

            var lc20 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 2
            };

            var lc21 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 2
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3, lc4, lc5, lc6, lc7, lc8, lc9, lc10, lc11, lc12, lc13, lc14, lc15, lc16, lc17, lc18, lc19, lc20, lc21 };

            var goal = new Goal()
            {
                Coefficients = new double[21] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Minimize
            };

            var expected = new Solution()
            {
                Decisions = new double[21] { 3.0, 2.0, 4.0, 1.0, 4.5, 6.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 2.0, 2.5, 5.0, 0.0, 0.0, 0.0, 0.0, 2.5, 0.0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 32.5
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        ///<summary> 
        ///Here's the first question from HW 5 in test form!
        ///It's a maximize question that's only one phase.
        ///</summary>
        [TestMethod()]
        public void HW5Question1MaxOnePhase()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 2, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 32
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 18
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 3 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 36
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 80, 70 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 14, 4 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 1400
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        ////<summary> 
        ////Here's the second question from HW 5 in test form!
        ////It's a maximize question. 
        ////</summary>
        ////<summary> 
        ////Here's the second question from HW 5 in test form!
        ////It's a maximize question. 
        ////</summary>
        [TestMethod()]
        public void HW5QuestionTwoScrews()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[12] { 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 1000
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 1000
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 600
            };

            var lc4 = new LinearConstraint()
            {
                Coefficients = new double[12] { 0.6, -0.4, -0.4, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 0
            };

            var lc5 = new LinearConstraint()
            {
                Coefficients = new double[12] { -0.2, 0.8, -0.2, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 0
            };

            var lc6 = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 0, 0, 0.8, -0.2, -0.2, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 0
            };

            var lc7 = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 0, 0, -0.4, 0.6, -0.4, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 0
            };

            var lc8 = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 0, 0, 0, 0, 0, 0.5, -0.5, -0.5, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 0
            };

            var lc9 = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 0, 0, 0, 0, 0, -0.1, 0.9, -0.1, 0, 0, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 0
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3, lc4, lc5, lc6, lc7, lc8, lc9 };

            var goal = new Goal()
            {
                Coefficients = new double[12] { .1, .3, .42, -.25, -.05, -.07, -.15, .05, .17, -.3, -.1, .02 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[12] { 1000, 400, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 472
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }



        //From Rees
        [TestMethod]
        public void ReesUnbounded()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, -1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 1
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 2
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 1, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Quality = SolutionQuality.Unbounded
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            Assert.AreEqual(expected.Quality, actual.Quality);
        }

        // From Rees
        [TestMethod]
        public void ReesInfeasible()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 2 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 8
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 3, 2 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 12
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 3 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 13
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 1, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Quality = SolutionQuality.Infeasible
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            Assert.AreEqual(expected.Quality, actual.Quality);
        }

        [TestMethod()]
        public void DegeneracyTest()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[3] { 1, 1, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 1
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[3] { 0, -1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 0
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2 };

            var goal = new Goal()
            {
                Coefficients = new double[3] { 1, 1, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[3] { 0, 1, 1 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 2
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        /////<summary> 
        /////Here's the third question from HW 5 in test form!
        /////It's a maximize question. 
        /////</summary>
        [TestMethod()]
        public void HW5QuestionThirdBoats()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[] { 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 12
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 18
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 10
            };

            var lc4 = new LinearConstraint()
            {
                Coefficients = new double[12] { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 17
            };

            var lc5 = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 16
            };

            var lc6 = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 25
            };

            var lc7 = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 15
            };

            var lc8 = new LinearConstraint()
            {
                Coefficients = new double[12] { 493, 0, 0, 672, 0, 0, 550, 0, 0, 375, 0, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 6750
            };

            var lc9 = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 493, 0, 0, 672, 0, 0, 550, 0, 0, 375, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 8000
            };

            var lc10 = new LinearConstraint()
            {
                //Coefficients = new double[12] { 10, 0, 493, 0, 0, 672, 0, 0, 550, 0, 0, 375 },
                Coefficients = new double[12] { 0, 0, 493, 0, 0, 672, 0, 0, 550, 0, 0, 375 },

                Relationship = Relationship.LessThanOrEquals,
                Value = 5150
            };


            var lc11 = new LinearConstraint()
            {
                Coefficients = new double[12] { -1, 12.0 / 18.0, 0, -1, 12.0 / 18.0, 0, -1, 12.0 / 18.0, 0, -1, 12.0 / 18.0, 0 },
                Relationship = Relationship.Equals,
                Value = 0
            };

            var lc12 = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, -1, 1.8, 0, -1, 1.8, 0, -1, 1.8, 0, -1, 1.8 },
                Relationship = Relationship.Equals,
                Value = 0
            };

            var lc13 = new LinearConstraint()
            {
                Coefficients = new double[12] { 5.0 / 6.0, 0, -1, 5.0 / 6.0, 0, -1, 5.0 / 6.0, 0, -1, 5.0 / 6.0, 0, -1 },
                Relationship = Relationship.Equals,
                Value = 0
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3, lc4, lc5, lc6, lc7, lc8, lc9, lc10, lc11, lc12, lc13 };

            var goal = new Goal()
            {
                Coefficients = new double[12] { 340, 340, 340, 368, 368, 368, 350, 350, 350, 300, 300, 300 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[12] { 0, 10.593220, 6.140351, 1.229508, 0, 0.197, 10.770492, 0, 3.596, 0, 7.406780, 0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 13485.34
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }


        ///<summary> 
        ///From a random powerpoint somwhere on the internet. 
        ///</summary>
        [TestMethod()]
        public void SillyConcrete()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[] { 1, 2, 10, 16 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 800
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[] { 1.5, 2, 3, 5 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 1000
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[] { 0.5, 0.6, 1, 2 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 340
            };


            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[] { 8, 14, 30, 50 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[] { 400, 200, 0, 0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 6000
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        ///// <summary>
        ///// This test has 5 decision variables and constraints. It is a two-phase maximize
        /////</summary>
        [TestMethod()]
        public void FiveVariables()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[5] { 1, 1, 1, 0, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 30
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[5] { 0, 0, 1, 1, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 15
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[5] { 0, 0, 0, 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 20
            };

            var lc4 = new LinearConstraint()
            {
                Coefficients = new double[5] { 1, 0, 0, 0, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 10
            };

            var lc5 = new LinearConstraint()
            {
                Coefficients = new double[5] { 0, 1, 1, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 5
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3, lc4, lc5 };

            var goal = new Goal()
            {
                Coefficients = new double[5] { 2, 1, 4, 3, 2 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[5] { 0, 0, 30, 10, 10 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                //OptimalValue = 2160.0 / 31.0
                OptimalValue = 170
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            //CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.IsTrue(ApproxEqualArray(expected.Decisions, actual.Decisions));
            //Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.IsTrue(ApproxEqual(expected.OptimalValue, actual.OptimalValue));
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }


        /// <summary>
        /// This test has 5 decision variables and constraints. It is a two phase maximize that works!
        ///</summary>
        [TestMethod()]
        public void FiveVariablesSilliness()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[5] { 1, 1, 1, 0, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 30
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[5] { 0, 0, 1, 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 15
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[5] { 0, 0, 0, 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 20
            };

            var lc4 = new LinearConstraint()
            {
                Coefficients = new double[5] { 1, 0, 0, 0, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 10
            };

            var lc5 = new LinearConstraint()
            {
                Coefficients = new double[5] { 0, 1, 1, 0, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 5
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3, lc4, lc5 };

            var goal = new Goal()
            {
                Coefficients = new double[5] { 2, 1, 4, 3, 2 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[5] { 30, 0, 0, 15, 0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                //OptimalValue = 2160.0 / 31.0
                OptimalValue = 105
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            //CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.IsTrue(ApproxEqualArray(expected.Decisions, actual.Decisions));
            //Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.IsTrue(ApproxEqual(expected.OptimalValue, actual.OptimalValue));
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        /// <summary>
        /// http://ocw.nctu.edu.tw/upload/classbfs1210015134169732.pdf
        /// ///</summary>
        [TestMethod()]
        public void OcwNctuEdu()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[] { 1, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 4
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[] { 0, 2 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 12
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[] { 3, 2 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 18
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[] { 3, 5 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[] { 2, 6 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 36
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            //CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.IsTrue(ApproxEqualArray(expected.Decisions, actual.Decisions));
            //Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.IsTrue(ApproxEqual(expected.OptimalValue, actual.OptimalValue));
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }


        /// <summary>
        /// I was googling around and found this one
        /// ///</summary>
        [TestMethod()]
        public void SomeRandomPowerpoint()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[] { 1, 2, 10, 16 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 800
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[] { 1.5, 2, 3, 5 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 1000
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[] { 0.5, 0.6, 1, 2 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 340
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[] { 8, 14, 30, 50 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[] { 400, 200, 0, 0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 6000
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            //CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.IsTrue(ApproxEqualArray(expected.Decisions, actual.Decisions));
            //Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.IsTrue(ApproxEqual(expected.OptimalValue, actual.OptimalValue));
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }


        /// <summary>
        /// This test has 3 decision variables and constraints. It is a two phase maximize
        ///</summary>
        [TestMethod()]
        public void ThreeVariablesSilliness()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[] { 1, 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 30
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[] { 1, 0, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 15
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[] { 0, 1, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 20
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[] { 2, 1, 4 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[] { 0, 0, 30 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                //OptimalValue = 2160.0 / 31.0
                OptimalValue = 120
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            //CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.IsTrue(ApproxEqualArray(expected.Decisions, actual.Decisions));
            //Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.IsTrue(ApproxEqual(expected.OptimalValue, actual.OptimalValue));
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        private static bool ApproxEqual(double a, double b)
        {
            return Math.Abs(a - b) < 0.0001;
        }

        private static bool ApproxEqualArray(ICollection<double> a, ICollection<double> b)
        {
            if (a.Count != b.Count)
                return false;

            for (var i = 0; i < a.Count; i++)
            {
                if (!ApproxEqual(a.ElementAt(i), b.ElementAt(i)))
                    return false;
            }
            return true;
        }
    }


}