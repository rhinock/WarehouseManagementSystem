using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WMS.Mappers
{
    //public abstract class BaseMapper<TFrom, TTo> where TTo : new()
    //{
    //    public BaseMapper()
    //    {
    //        properties = new Dictionary<LambdaExpression, LambdaExpression>();
    //        InitMap();
    //    }
    //    protected Dictionary<LambdaExpression, LambdaExpression> properties;
    //    protected void Map<TProperty>(Expression<Func<TFrom, TProperty>> fromSelector, Expression<Func<TTo, TProperty>> toSelector)
    //    {
    //        properties.Add(fromSelector, toSelector);
    //    }
    //    protected abstract void InitMap();
    //    public void Convert(TFrom from, TTo to)
    //    {
    //        ParameterExpression parameterExpressionFrom = Expression.Parameter(typeof(TFrom), "from");
    //        ParameterExpression parameterExpressionTo = Expression.Parameter(typeof(TTo), "to");

    //        List<Expression> expressions = new List<Expression>();

    //        foreach (var propertyMap in properties)
    //        {
    //            expressions.Add(Expression.Assign(propertyMap.Value.Body, propertyMap.Key.Body));
    //        }

    //        BlockExpression body = Expression.Block(new[] { parameterExpressionFrom, parameterExpressionTo }, expressions);
    //        Action<TFrom, TTo> action = Expression.Lambda<Action<TFrom, TTo>>(body, parameterExpressionFrom, parameterExpressionTo).Compile();
    //        action(from, to);
    //    }
    //    //public TTo Convert(TFrom from)
    //    //{
    //    //    List<MemberBinding> memberBindings = new List<MemberBinding>();
    //    //    ParameterExpression parameterExpression = Expression.Parameter(typeof(TFrom), "from");

    //    //    foreach (var propertyMap in properties)
    //    //    {
    //    //        MemberExpression fromProperty = propertyMap.Key.Body as MemberExpression;
    //    //        MemberInfo property = fromProperty.Member;
    //    //        memberBindings.Add(Expression.Bind(property, propertyMap.Value.Body));
    //    //    }

    //    //    NewExpression constructor = Expression.New(typeof(TTo));
    //    //    MemberInitExpression memberInitExpression = Expression.MemberInit(constructor, memberBindings);
    //    //    Func<TFrom, TTo> func = Expression.Lambda<Func<TFrom, TTo>>(memberInitExpression, parameterExpression).Compile();

    //    //    return func(from);
    //    //}
    //}
}
