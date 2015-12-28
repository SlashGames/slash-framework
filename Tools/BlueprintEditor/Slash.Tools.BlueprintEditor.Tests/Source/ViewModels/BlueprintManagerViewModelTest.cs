// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintManagerViewModelTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Tests.Source.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MonitoredUndo;

    using NUnit.Framework;

    using Slash.ECS.Blueprints;

    using global::BlueprintEditor.ViewModels;

    public class BlueprintManagerViewModelTest
    {
        #region Fields

        private HierarchicalBlueprintManager testBlueprintManager;

        private BlueprintManagerViewModel testViewModel;

        #endregion

        #region Public Methods and Operators

        [SetUp]
        public void SetUp()
        {
            this.testBlueprintManager = new HierarchicalBlueprintManager();
            this.testBlueprintManager.AddChild(new BlueprintManager());
            this.testViewModel = new BlueprintManagerViewModel(new List<Type>(), this.testBlueprintManager);
            this.testViewModel.CurrentBlueprintManager = (BlueprintManager)this.testBlueprintManager.Children.First();

            UndoService.Current.Clear();
        }

        [Test]
        public void TestRedoCreateNewBlueprint()
        {
            this.AddBlueprint("NewBlueprint");

            // Check redo.
            this.AssertBlueprintCount(1);
            this.Undo();
            this.AssertBlueprintCount(0);
            this.Redo();
            this.AssertBlueprintCount(1);
        }

        [Test]
        public void TestRedoRemoveBlueprint()
        {
            // Add blueprint.
            const string BlueprintId = "NewBlueprint";
            this.AddBlueprint(BlueprintId);

            // Remove blueprint.
            this.RemoveBlueprint(BlueprintId);

            // Check redo.
            this.AssertBlueprintCount(0);
            this.Undo();
            this.AssertBlueprintCount(1);
            this.Redo();
            this.AssertBlueprintCount(0);
        }

        [Test]
        public void TestUndoCreateNewBlueprint()
        {
            const string BlueprintName = "NewBlueprint";
            this.AddBlueprint(BlueprintName);

            Assert.AreEqual(1, UndoService.Current[this.testViewModel].UndoStack.Count());

            var change = UndoService.Current[this.testViewModel].UndoStack.FirstOrDefault();
            Assert.AreEqual(string.Format("Add blueprint '{0}'", BlueprintName), change.Description);

            // Check undo.
            this.AssertBlueprintCount(1);
            this.Undo();
            this.AssertBlueprintCount(0);
        }

        [Test]
        public void TestUndoRemoveBlueprint()
        {
            // Add blueprint.
            const string BlueprintId = "NewBlueprint";
            this.AddBlueprint(BlueprintId);

            // Remove blueprint.
            this.RemoveBlueprint(BlueprintId);

            // Check undo.
            this.AssertBlueprintCount(0);
            this.Undo();
            this.AssertBlueprintCount(1);
        }

        #endregion

        #region Methods

        private void AddBlueprint(string blueprintId)
        {
            this.testViewModel.NewBlueprintId = blueprintId;
            this.testViewModel.CreateNewBlueprint();
        }

        private void AssertBlueprintCount(int count)
        {
            Assert.AreEqual(count, this.testViewModel.Blueprints.Count);
            Assert.AreEqual(count, this.testBlueprintManager.Blueprints.Count());
        }

        private void Redo()
        {
            UndoService.Current[this.testViewModel].Redo();
        }

        private void RemoveBlueprint(string blueprintId)
        {
            this.testViewModel.RemoveBlueprint(blueprintId);
        }

        private void Undo()
        {
            UndoService.Current[this.testViewModel].Undo();
        }

        #endregion
    }
}