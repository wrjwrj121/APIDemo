using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WeBuy.Common.Eenu;

namespace WeBuy.Common.DB
{
    public class EFCoreExtends<T>
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T));
        public Expression<Func<T, bool>> ParserConditions(IEnumerable<UosoConditions> conditions)
        {

            //将条件转化成表达是的Body
            var query = ParseExpressionBody(conditions);
            return Expression.Lambda<Func<T, bool>>(query, parameter);
        }

        public class UosoConditions
        {
            /// <summary>
            /// 字段名称
            /// </summary>
            public string Key { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public string Value { get; set; }
            /// <summary>
            /// 值类型
            /// </summary>
            public string ValueType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public UosoOperatorEnum Operator { get; set; }
        }

        private Expression ParseExpressionBody(IEnumerable<UosoConditions> conditions)
        {
            if (conditions == null || conditions.Count() == 0)
            {
                return Expression.Constant(true, typeof(bool));
            }
            else if (conditions.Count() == 1)
            {
                return ParseCondition(conditions.First());
            }
            else
            {
                Expression left = ParseCondition(conditions.First());
                Expression right = ParseExpressionBody(conditions.Skip(1));
                return Expression.AndAlso(left, right);
            }
        }

        private Expression ParseCondition(UosoConditions condition)
        {
            ParameterExpression p = parameter;
            Expression key = Expression.Property(p, condition.Key);
            Expression value = Expression.Constant(condition.Value);
            switch (condition.Operator)
            {
                case UosoOperatorEnum.Contains:
                    return Expression.Call(key, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), value);
                case UosoOperatorEnum.Equal:
                    return Expression.Equal(key, Expression.Convert(value, key.Type));
                case UosoOperatorEnum.Greater:
                    return Expression.GreaterThan(key, Expression.Convert(value, key.Type));
                case UosoOperatorEnum.GreaterEqual:
                    return Expression.GreaterThanOrEqual(key, Expression.Convert(value, key.Type));
                case UosoOperatorEnum.Less:
                    return Expression.LessThan(key, Expression.Convert(value, key.Type));
                case UosoOperatorEnum.LessEqual:
                    return Expression.LessThanOrEqual(key, Expression.Convert(value, key.Type));
                case UosoOperatorEnum.NotEqual:
                    return Expression.NotEqual(key, Expression.Convert(value, key.Type));
                case UosoOperatorEnum.In:
                    return ParaseIn(p, condition);
                case UosoOperatorEnum.Between:
                    return ParaseBetween(p, condition);
                default:
                    throw new NotImplementedException("不支持此操作");
            }
        }

        private Expression ParaseBetween(ParameterExpression parameter, UosoConditions conditions)
        {
            ParameterExpression p = parameter;
            Expression key = Expression.Property(p, conditions.Key);
            var valueArr = conditions.Value.Split(',');
            if (valueArr.Length != 2)
            {
                throw new NotImplementedException("ParaseBetween参数错误");
            }
            try
            {
                int.Parse(valueArr[0]);
                int.Parse(valueArr[1]);
            }
            catch
            {
                throw new NotImplementedException("ParaseBetween参数只能为数字");
            }
            Expression expression = Expression.Constant(true, typeof(bool));
            //开始位置
            Expression startvalue = Expression.Constant(int.Parse(valueArr[0]));
            Expression start = Expression.GreaterThanOrEqual(key, Expression.Convert(startvalue, key.Type));

            Expression endvalue = Expression.Constant(int.Parse(valueArr[1]));
            Expression end = Expression.GreaterThanOrEqual(key, Expression.Convert(endvalue, key.Type));
            return Expression.AndAlso(start, end);
        }

        private Expression ParaseIn(ParameterExpression parameter, UosoConditions conditions)
        {
            ParameterExpression p = parameter;
            Expression key = Expression.Property(p, conditions.Key);
            var valueArr = conditions.Value.Split(',');
            Expression expression = Expression.Constant(true, typeof(bool));
            foreach (var itemVal in valueArr)
            {
                Expression value = Expression.Constant(itemVal);
                Expression right = Expression.Equal(key, Expression.Convert(value, key.Type));

                expression = Expression.Or(expression, right);
            }
            return expression;
        }
    }
    
}
