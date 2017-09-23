using System;
using System.Linq;
using System.Linq.Expressions;

namespace Shopping.Ultilities
{
    public abstract class FilterDto<T>
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string OrderBy { get; set; }
        public OrderType? OrderType { get; set; }

        public abstract IQueryable<T> ApplyTo (IQueryable<T> source);

        public IQueryable<T> SkipAndTake(IQueryable<T> source)
        {
            if (Skip.HasValue && Skip > 0)
            {
                source = source.Skip((int)Skip);
            }
            if (Take.HasValue && Take > 0)
            {
                source = source.Take((int)Take);
            }

            return source;
        }

        public IQueryable<T> Order(IQueryable<T> source)
        {
            OrderType = OrderType ?? Ultilities.OrderType.Asc;
            var command = OrderType == Ultilities.OrderType.Desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");
            var property = type.GetProperty("Id");
            if (!string.IsNullOrEmpty(OrderBy) && OrderType != Ultilities.OrderType.None)
            {
                property = type.GetProperty(OrderBy);
            }
            var propertyAccess = Expression.MakeMemberAccess(parameter, property ?? throw new InvalidOperationException());
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command,
                new[] { type, property.PropertyType },
                source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<T>(resultExpression);
        }
    }

    public enum OrderType
    {
        None = 0,
        Desc = 1,
        Asc = 2
    }
}