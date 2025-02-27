﻿using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace NGql.Core.Tests
{
    public class QueryTests
    {
        [Fact]
        public void Ctor_Sets_Name()
        {
            // arrange
            var query = new Query("name");

            // assert
            query.Name.Should().Be("name");
            query.Alias.Should().BeNull();

            query.FieldsList.Should().BeEmpty();
            query.Arguments.Should().BeEmpty();
        }

        [Fact]
        public void Ctor_Sets_Alias()
        {
            // arrange
            var query = new Query("name", "alias");

            // assert
            query.Name.Should().Be("name");
            query.Alias.Should().Be("alias");

            query.FieldsList.Should().BeEmpty();
            query.Arguments.Should().BeEmpty();
        }

        [Fact]
        public void AliasAs_Sets_Alias()
        {
            // arrange
            var query = new Query("name");

            // act
            query.AliasAs("alias").Select("id");

            // assert
            query.Alias.Should().Be("alias");
        }

        [Fact]
        public void Select_String_AddsToSelectList()
        {
            // arrange
            var query = new Query("name");

            // act
            query.Select("id", "name");

            // assert
            query.FieldsList.Should().BeEquivalentTo(new[] { "id", "name" });
        }

        [Fact]
        public void Select_List_AddsToSelectList()
        {
            // arrange
            var query = new Query("name");

            // act
            query.Select(new List<string> {"id", "name"});

            // assert
            query.FieldsList.Should().BeEquivalentTo(new[] { "id", "name" });
        }

        [Fact]
        public void Select_ChainOfCalls_AddsToSelectList()
        {
            // arrange
            var query = new Query("name");

            // act
            query
                .Select("id")
                .Select(new List<string> { "name"});

            // assert
            query.FieldsList.Should().BeEquivalentTo(new[] { "id", "name" });
        }

        [Fact]
        public void ToString_Returns_Query()
        {
            // arrange
            var usersQuery = new Query("users")
                .Select("id", "name");

            // act
            string queryText = usersQuery;

            // assert
            queryText.Should().Be(@"query users{
    id
    name
}");
        }

        [Fact]
        public void ToString_Returns_SubQuery()
        {
            // arrange
            var query = new Query("myQuery");
            var usersQuery = new Query("users")
                .Select("id", "name");

            // act
            string queryText = query
                .Select(usersQuery);

            // assert
            queryText.Should().Be(@"query myQuery{
    users{
        id
        name
    }
}");
        }

        [Fact]
        public void ToString_Returns_SubQueryAlias()
        {
            // arrange
            var query = new Query("myQuery");
            var usersQuery = new Query("users", "alias")
                .Select("id", "name");

            // act
            string queryText = query
                .Select(usersQuery);

            // assert
            queryText.Should().Be(@"query myQuery{
    alias:users{
        id
        name
    }
}");
        }

        [Fact]
        public void ToString_Includes_SubQueryAlias()
        {
            // act
            string queryText = new Query("myQuery")
                .Include("users", x => x
                    .AliasAs("alias")
                    .Select("id", "name")
                );

            // assert
            queryText.Should().Be(@"query myQuery{
    alias:users{
        id
        name
    }
}");
        }
    }
}
