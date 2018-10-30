﻿using System.Linq;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers
{
    public class DataTableSerializerTests
    {
        private DataTableSerializer _dataTableSerializer;
        private SaveFilter _saveFilter;

        [SetUp]
        public void Setup()
        {
            _saveFilter = new SaveFilter();
            _dataTableSerializer = new DataTableSerializer(_saveFilter);
        }

        [Test]
        public void AllItemsSerialized()
        {
            var model = CreateConnectionTreeModel();
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows.Count, Is.EqualTo(model.GetRecursiveChildList().Count()));
        }

        [Test]
        public void ReturnsEmptyDataTableWhenGivenEmptyConnectionTreeModel()
        {
            var model = new ConnectionTreeModel();
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows.Count, Is.EqualTo(0));
        }

        [Test]
        public void UsernameSerializedWhenSaveSecurityAllowsIt()
        {
            var model = CreateConnectionTreeModel();
            _saveFilter.SaveUsername = true;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["Username"], Is.Not.EqualTo(""));
        }

        [Test]
        public void DomainSerializedWhenSaveSecurityAllowsIt()
        {
            var model = CreateConnectionTreeModel();
            _saveFilter.SaveDomain = true;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["DomainName"], Is.Not.EqualTo(""));
        }

        [Test]
        public void PasswordSerializedWhenSaveSecurityAllowsIt()
        {
            var model = CreateConnectionTreeModel();
            _saveFilter.SavePassword = true;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["Password"], Is.Not.EqualTo(""));
        }

        [Test]
        public void InheritanceSerializedWhenSaveSecurityAllowsIt()
        {
            var model = CreateConnectionTreeModel();
            _saveFilter.SaveInheritance = true;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["InheritUsername"], Is.Not.EqualTo(""));
        }



        [Test]
        public void UsernameNotSerializedWhenSaveSecurityDisabled()
        {
            var model = CreateConnectionTreeModel();
            _saveFilter.SaveUsername = false;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["Username"], Is.EqualTo(""));
        }

        [Test]
        public void DomainNotSerializedWhenSaveSecurityDisabled()
        {
            var model = CreateConnectionTreeModel();
            _saveFilter.SaveDomain = false;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["DomainName"], Is.EqualTo(""));
        }

        [Test]
        public void PasswordNotSerializedWhenSaveSecurityDisabled()
        {
            var model = CreateConnectionTreeModel();
            _saveFilter.SavePassword = false;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["Password"], Is.EqualTo(""));
        }

        [Test]
        public void InheritanceNotSerializedWhenSaveSecurityDisabled()
        {
            var model = CreateConnectionTreeModel();
            _saveFilter.SaveInheritance = false;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["InheritUsername"], Is.False);
        }

        [Test]
        public void CanSerializeEmptyConnectionInfo()
        {
            var dataTable = _dataTableSerializer.Serialize(new ConnectionInfo());
            Assert.That(dataTable.Rows.Count, Is.EqualTo(1));
        }


        private ConnectionTreeModel CreateConnectionTreeModel()
        {
            var builder = new ConnectionTreeModelBuilder();
            return builder.Build();
        }
    }
}