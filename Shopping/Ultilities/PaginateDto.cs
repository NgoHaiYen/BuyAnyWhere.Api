using System.Linq;
using System.Linq.Expressions;

namespace Shopping.Ultilities
{
    public class PaginateDto
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string OrderBy { get; set; }
        public OrderType? OrderType { get; set; }

        public IQueryable<T> SkipAndTake<T>(IQueryable<T> source)
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

        public IQueryable<T> Order<T>(IQueryable<T> Source)
        {
            OrderType = OrderType ?? Ultilities.OrderType.Asc;
            var Command = OrderType == Ultilities.OrderType.Desc ? "OrderByDescending" : "OrderBy";
            var Type = typeof(T);
            var Parameter = Expression.Parameter(Type, "p");
            var Property = Type.GetProperty("Id");
            if (!string.IsNullOrEmpty(OrderBy) && OrderType != Ultilities.OrderType.None)
            {
                Property = Type.GetProperty(OrderBy);
            }
            var PropertyAccess = Expression.MakeMemberAccess(Parameter, Property);
            var OrderByExpression = Expression.Lambda(PropertyAccess, Parameter);
            var ResultExpression = Expression.Call(typeof(Queryable), Command,
                new[] { Type, Property.PropertyType },
                Source.Expression, Expression.Quote(OrderByExpression));
            return Source.Provider.CreateQuery<T>(ResultExpression);
        }
    }


    public enum OrderType
    {
        None = 0,
        Desc = 1,
        Asc = 2
    }
}