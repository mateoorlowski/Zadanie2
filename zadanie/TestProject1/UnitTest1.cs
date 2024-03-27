using LegacyApp;

namespace TestProject1
{
    using NUnit.Framework;

    [TestFixture]
    public class UserServiceTests
    {
        [Test]
        public void ShouldReturnTrueWhenAddUserValidValues()
        {
            // Arrange
            var userService = new UserService();

            // Act
            bool result = userService.AddUser("John", "Doe", "johndoe@gmail.com", new DateTime(1982, 3, 21), 1);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnFalseWhenAddUserInvalidValues()
        {
            // Arrange
            var userService = new UserService();

            // Act
            bool result = userService.AddUser("", "", "invalidemail", DateTime.Now, 1);

            // Assert
            Assert.IsFalse(result);
        }
    }
}

