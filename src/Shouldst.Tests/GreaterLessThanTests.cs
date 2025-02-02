namespace Shouldst.Tests;

public class GreaterLessThanTests
{
    [Test]
    public void ShouldBeGreaterWithLessThanPrimitiveSucceeds()
    {
        5.ShouldBeGreaterThan(4);
    }

    [Test]
    public void ShouldBeGreaterWithEqualPrimitiveThrows()
    {
        Assert.Throws<ShouldException>(() => 5.ShouldBeGreaterThan(5));
    }

    [Test]
    public void ShouldBeGreaterWithGreaterPrimitiveThrows()
    {
        Assert.Throws<ShouldException>(() => 5.ShouldBeGreaterThan(10));
    }

    [Test]
    public void ShouldBeGreaterOrEqualWithLessThanPrimitiveSucceeds()
    {
        5.ShouldBeGreaterThanOrEqualTo(4);
    }

    [Test]
    public void ShouldBeGreaterOrEqualWithEqualPrimitiveSucceeds()
    {
        5.ShouldBeGreaterThanOrEqualTo(5);
    }

    [Test]
    public void ShouldBeGreaterOrEqualWithGreaterPrimitiveThrows()
    {
        Assert.Throws<ShouldException>(() => 5.ShouldBeGreaterThanOrEqualTo(10));
    }

    [Test]
    public void ShouldBeGreaterWithLessThanComplexSucceeds()
    {
        new MyRecord(5).ShouldBeGreaterThan(new MyRecord(4));
    }

    [Test]
    public void ShouldBeGreaterWithEqualComplexThrows()
    {
        Assert.Throws<ShouldException>(() => new MyRecord(5).ShouldBeGreaterThan(new MyRecord(5)));
    }

    [Test]
    public void ShouldBeGreaterWithGreaterComplexThrows()
    {
        Assert.Throws<ShouldException>(() => new MyRecord(5).ShouldBeGreaterThan(new MyRecord(10)));
    }

    [Test]
    public void ShouldBeGreaterOrEqualWithLessThanComplexSucceeds()
    {
        new MyRecord(5).ShouldBeGreaterThanOrEqualTo(new MyRecord(4));
    }

    [Test]
    public void ShouldBeGreaterOrEqualWithEqualComplexSucceeds()
    {
        new MyRecord(5).ShouldBeGreaterThanOrEqualTo(new MyRecord(5));
    }

    [Test]
    public void ShouldBeGreaterOrEqualWithGreaterComplexThrows()
    {
        Assert.Throws<ShouldException>(() => new MyRecord(5).ShouldBeGreaterThanOrEqualTo(new MyRecord(10)));
    }







    [Test]
    public void ShouldBeLessWithGreaterThanPrimitiveSucceeds()
    {
        4.ShouldBeLessThan(5);
    }

    [Test]
    public void ShouldBeLessWithEqualPrimitiveThrows()
    {
        Assert.Throws<ShouldException>(() => 5.ShouldBeLessThan(5));
    }

    [Test]
    public void ShouldBeLessWithLessPrimitiveThrows()
    {
        Assert.Throws<ShouldException>(() => 10.ShouldBeLessThan(5));
    }

    [Test]
    public void ShouldBeLessOrEqualWithGreaterThanPrimitiveSucceeds()
    {
        4.ShouldBeLessThanOrEqualTo(5);
    }

    [Test]
    public void ShouldBeLessOrEqualWithEqualPrimitiveSucceeds()
    {
        5.ShouldBeLessThanOrEqualTo(5);
    }

    [Test]
    public void ShouldBeLessOrEqualWithLessPrimitiveThrows()
    {
        Assert.Throws<ShouldException>(() => 10.ShouldBeLessThanOrEqualTo(5));
    }

    [Test]
    public void ShouldBeLessWithGreaterThanComplexSucceeds()
    {
        new MyRecord(4).ShouldBeLessThan(new MyRecord(5));
    }

    [Test]
    public void ShouldBeLessWithEqualComplexThrows()
    {
        Assert.Throws<ShouldException>(() => new MyRecord(5).ShouldBeLessThan(new MyRecord(5)));
    }

    [Test]
    public void ShouldBeLessWithLessComplexThrows()
    {
        Assert.Throws<ShouldException>(() => new MyRecord(10).ShouldBeLessThan(new MyRecord(5)));
    }

    [Test]
    public void ShouldBeLessOrEqualWithGreaterThanComplexSucceeds()
    {
        new MyRecord(4).ShouldBeLessThanOrEqualTo(new MyRecord(5));
    }

    [Test]
    public void ShouldBeLessOrEqualWithEqualComplexSucceeds()
    {
        new MyRecord(5).ShouldBeLessThanOrEqualTo(new MyRecord(5));
    }

    [Test]
    public void ShouldBeLessOrEqualWithLessComplexThrows()
    {
        Assert.Throws<ShouldException>(() => new MyRecord(10).ShouldBeLessThanOrEqualTo(new MyRecord(5)));
    }

    private record MyRecord(int Value) : IComparable<MyRecord>
    {
        public int CompareTo(MyRecord? other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (other is null)
            {
                return 1;
            }

            return Value.CompareTo(other.Value);
        }
    }
}
