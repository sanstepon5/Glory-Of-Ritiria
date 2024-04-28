﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.6.6
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from D:\programs\Godot\GloryOfRitiria\Parsers\StellarSystemParser\StellarGenerator.g4 by ANTLR 4.6.6

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace StellarSystemParser {

    using System.Globalization;
    using GloryOfRitiria.Scripts.Utils;

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IStellarGeneratorVisitor{Result}"/>,
/// which can be extended to create a visitor which only needs to handle a subset
/// of the available methods.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.6.6")]
[System.CLSCompliant(false)]
public partial class StellarGeneratorBaseVisitor<Result> : AbstractParseTreeVisitor<Result>, IStellarGeneratorVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.file"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitFile([NotNull] StellarGeneratorParser.FileContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.stellar_system"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitStellar_system([NotNull] StellarGeneratorParser.Stellar_systemContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.stellar_system_body"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitStellar_system_body([NotNull] StellarGeneratorParser.Stellar_system_bodyContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.star"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitStar([NotNull] StellarGeneratorParser.StarContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.star_body"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitStar_body([NotNull] StellarGeneratorParser.Star_bodyContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.celestial_body"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCelestial_body([NotNull] StellarGeneratorParser.Celestial_bodyContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.celestial_body_body"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCelestial_body_body([NotNull] StellarGeneratorParser.Celestial_body_bodyContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.satellites"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSatellites([NotNull] StellarGeneratorParser.SatellitesContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.shipyards"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitShipyards([NotNull] StellarGeneratorParser.ShipyardsContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.shipyard"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitShipyard([NotNull] StellarGeneratorParser.ShipyardContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.shipyard_body"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitShipyard_body([NotNull] StellarGeneratorParser.Shipyard_bodyContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.ships"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitShips([NotNull] StellarGeneratorParser.ShipsContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.ship"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitShip([NotNull] StellarGeneratorParser.ShipContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.ship_body"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitShip_body([NotNull] StellarGeneratorParser.Ship_bodyContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.modules"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitModules([NotNull] StellarGeneratorParser.ModulesContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.module"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitModule([NotNull] StellarGeneratorParser.ModuleContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitName([NotNull] StellarGeneratorParser.NameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.icon"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitIcon([NotNull] StellarGeneratorParser.IconContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.body_type"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitBody_type([NotNull] StellarGeneratorParser.Body_typeContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.ship_size"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitShip_size([NotNull] StellarGeneratorParser.Ship_sizeContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.distance_from"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitDistance_from([NotNull] StellarGeneratorParser.Distance_fromContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.angle"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitAngle([NotNull] StellarGeneratorParser.AngleContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.pull"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPull([NotNull] StellarGeneratorParser.PullContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.body_distance"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitBody_distance([NotNull] StellarGeneratorParser.Body_distanceContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.building_progress"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitBuilding_progress([NotNull] StellarGeneratorParser.Building_progressContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.star_class"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitStar_class([NotNull] StellarGeneratorParser.Star_classContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.discovery_status"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitDiscovery_status([NotNull] StellarGeneratorParser.Discovery_statusContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.id"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitId([NotNull] StellarGeneratorParser.IdContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.text"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitText([NotNull] StellarGeneratorParser.TextContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.inty"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitInty([NotNull] StellarGeneratorParser.IntyContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="StellarGeneratorParser.floaty"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitFloaty([NotNull] StellarGeneratorParser.FloatyContext context) { return VisitChildren(context); }
}
} // namespace StellarSystemParser
