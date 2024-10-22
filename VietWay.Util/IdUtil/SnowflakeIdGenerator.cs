using IdGen;

namespace VietWay.Util.IdUtil
{
    public class SnowflakeIdGenerator : IIdGenerator
    {
        private readonly IdGenerator _generator = new(0);
        public string GenerateId()
        {
            return _generator.CreateId().ToString();
        }
    }
}
