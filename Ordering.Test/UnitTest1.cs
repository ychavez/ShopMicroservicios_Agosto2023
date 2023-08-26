namespace Ordering.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {  
            //Arrange
            var nunmero1 = 1;
            var nunmero2 = 2;

            //Act
            var resultado = nunmero1 + nunmero2 + 1;

            //Assert
            Assert.Equal(3, resultado);

        }
    }
}