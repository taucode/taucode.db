﻿namespace TauCode.Db.DbValueConverters
{
    public class Int64ValueConverter : DbValueConverterBase
    {
        protected override object ToDbValueImpl(object value)
        {
            if (value is long longValue)
            {
                return longValue;
            }
            else if (value is int intValue)
            {
                return intValue;
            }

            return null;
        }

        protected override object FromDbValueImpl(object dbValue)
        {
            if (dbValue is long longValue)
            {
                return longValue;
            }

            return null;
        }
    }
}
