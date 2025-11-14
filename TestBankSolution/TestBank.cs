using BankProject;

namespace TestBankSolution
{
	public class TestBank
	{
		Bank bank;
		[SetUp]
		public void Setup()
		{
			bank = new Bank();
		}

		[Test]
		public void UjSzamla_NullNev_ArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => bank.UjSzamla(null, "1234"));
			
		}

		[Test]
		public void Ujszamla_NullSzamlaszam_ArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => bank.UjSzamla("Teszt Elek", null));
		}

		[Test]
		public void UjSzamla_ErvenyesErtekkel_NemDobKivetelt()
		{
			Assert.DoesNotThrow(() => bank.UjSzamla("Teszt Elek", "1234"));
		}

		[Test]
		public void UjSzamla_DuplikaltSzamlaszam_ArgumentException()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");
			Assert.Throws<ArgumentException>(() => bank.UjSzamla("Teszt Elek", "1234"));
		}

		[Test]
		public void UjSzamla_DuplikaltNev_NemDobKivetelt()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");
			Assert.DoesNotThrow(() => bank.UjSzamla("Gipsz Jakab", "5678"));
		}

		[Test]
		public void UjSzamla_ErvenyesErtekekkel_Egyenleg0()
		{
			bank.UjSzamla("Teszt Elek", "1234");
			Assert.That(bank.Egyenleg("1234"), Is.Zero);
		}

		[Test]
		public void Egyenleg_NullSzamlaszam_ArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => bank.Egyenleg(null));
		}
	}
}