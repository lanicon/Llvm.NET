﻿// -----------------------------------------------------------------------
// <copyright file="LocalVariableDeclaration.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;

using Ubiquity.ArgValidators;

namespace Kaleidoscope.Grammar.AST
{
    public class LocalVariableDeclaration
        : IVariableDeclaration
    {
        public LocalVariableDeclaration( SourceSpan location, string name, IExpression? initializer, bool compilerGenerated = false )
        {
            Location = location;
            Name = name;
            Initializer = initializer;
            CompilerGenerated = compilerGenerated;
        }

        public SourceSpan Location { get; }

        public string Name { get; }

        public IExpression? Initializer { get; }

        public bool CompilerGenerated { get; }

        public TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
            where TResult : class
        {
            return visitor.ValidateNotNull( nameof( visitor ) ).Visit( this );
        }

        public IEnumerable<IAstNode> Children
        {
            get
            {
                if( Initializer != null )
                {
                    yield return Initializer;
                }
            }
        }

        public override string ToString( )
        {
            var bldr = new StringBuilder();
            if( CompilerGenerated )
            {
                bldr.Append( "[CompilerGenerated]" );
            }

            bldr.Append( "Declare(" );
            bldr.Append( Name );
            if( Initializer != null )
            {
                bldr.Append( ", " );
                bldr.Append( Initializer );
            }

            bldr.Append( ')' );
            return bldr.ToString( );
        }
    }
}
