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

		[Test]
		public void Egyenleg_BetuASzamlaszamban_ArgumentException()
		{
			bank.UjSzamla("Teszt Elek", "1234");
			Assert.Throws<ArgumentException>(() => bank.Egyenleg("abcd"));
		}

		[Test]
		public void Egyenleg_NemLetezoSzamlaszam_HibasSzamlaszamException()
		{
            bank.UjSzamla("Teszt Elek", "1234");
			Assert.Throws<HibasSzamlaszamException>(() => bank.Egyenleg("9876"));
        }

		[Test]
		public void Egyenleg_ErvenyesSzamlaszammal_NemDobKivetelt()
		{
			bank.UjSzamla("Teszt Elek", "1234 - 5678");

			Assert.DoesNotThrow(() => bank.Egyenleg("1234 - 5678"));
		}

        [Test]
        public void Feltolt_NullSzamlaszam_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => bank.EgyenlegFeltolt(null, 100));
        }

        [Test]
        public void Feltolt_BetuASzamlaszamban_ArgumentException()
        {
            bank.UjSzamla("Teszt", "1234");
            Assert.Throws<ArgumentException>(() => bank.EgyenlegFeltolt("abc", 100));
        }

        [Test]
        public void Feltolt_NemLetezoSzamlaszam_HibasSzamlaszamException()
        {
            Assert.Throws<HibasSzamlaszamException>(() => bank.EgyenlegFeltolt("9999", 100));
        }

        [Test]
        public void Feltolt_Osszeg0_ArgumentException()
        {
            bank.UjSzamla("Teszt", "1234");
            Assert.Throws<ArgumentException>(() => bank.EgyenlegFeltolt("1234", 0));
        }

        [Test]
        public void Feltolt_Ervenyes_MegnovelEgyenleget()
        {
            bank.UjSzamla("Teszt", "1234");
            bank.EgyenlegFeltolt("1234", 500);

            Assert.That(bank.Egyenleg("1234"), Is.EqualTo(500));
        }

        [Test]
        public void Feltolt_UjabbFeltoltes_Hozzaadodik()
        {
            bank.UjSzamla("Teszt", "1234");
            bank.EgyenlegFeltolt("1234", 200);
            bank.EgyenlegFeltolt("1234", 300);

            Assert.That(bank.Egyenleg("1234"), Is.EqualTo(500));
        }

		[Test]
		public void Utal_NullHonnan_ArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => bank.Utal(null, "2222", 100));
		}

		[Test]
		public void Utal_NullHova_ArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => bank.Utal("1111", null, 100));
		}

		[Test]
		public void Utal_BetuASzamlaszamban_ArgumentException()
		{
			Assert.Throws<ArgumentException>(() => bank.Utal("abc", "1234", 100));
		}

		[Test]
		public void Utal_Osszeg0_ArgumentException()
		{
			Assert.Throws<ArgumentException>(() => bank.Utal("1111", "2222", 0));
		}

		[Test]
		public void Utal_NemLetezoSzamla_HibasSzamlaszamException()
		{
			bank.UjSzamla("Teszt", "1111");
			Assert.Throws<HibasSzamlaszamException>(() => bank.Utal("1111", "9999", 100));
		}

		[Test]
		public void Utal_NincsElegPenze_FalseTerVissza()
		{
			bank.UjSzamla("A", "1111");
			bank.UjSzamla("B", "2222");

			bank.EgyenlegFeltolt("1111", 50);

			Assert.False(bank.Utal("1111", "2222", 100));
		}

		[Test]
		public void Utal_ElegPenze_Atmegy()
		{
			bank.UjSzamla("A", "1111");
			bank.UjSzamla("B", "2222");

			bank.EgyenlegFeltolt("1111", 500);

			var result = bank.Utal("1111", "2222", 300);

			Assert.True(result);
			Assert.That(bank.Egyenleg("1111"), Is.EqualTo(200));
			Assert.That(bank.Egyenleg("2222"), Is.EqualTo(300));
		}
        [Test]
        public void Feltolt_UresSzamlaszam_ArgumentException()
        {
            Assert.Throws<HibasSzamlaszamException>(() => bank.EgyenlegFeltolt("", 100));
            Assert.Throws<HibasSzamlaszamException>(() => bank.EgyenlegFeltolt("   ", 100));
        }

        [Test]
        public void Feltolt_MasikSzamlaValtozatlan()
        {
            bank.UjSzamla("A", "1111");
            bank.UjSzamla("B", "2222");

            bank.EgyenlegFeltolt("1111", 300);

            Assert.That(bank.Egyenleg("1111"), Is.EqualTo(300));
            Assert.That(bank.Egyenleg("2222"), Is.EqualTo(0));
        }

        [Test]
        public void Utal_UresHonnan_ArgumentException()
        {
            Assert.Throws<HibasSzamlaszamException>(() => bank.Utal("", "2222", 100));
            Assert.Throws<HibasSzamlaszamException>(() => bank.Utal("   ", "2222", 100));
        }

        [Test]
        public void Utal_MasikSzamlaValtozatlan()
        {
            bank.UjSzamla("A", "1111");
            bank.UjSzamla("B", "2222");
            bank.UjSzamla("C", "3333");

            bank.EgyenlegFeltolt("1111", 500);
            bank.EgyenlegFeltolt("3333", 700);

            bank.Utal("1111", "2222", 200);

            Assert.That(bank.Egyenleg("1111"), Is.EqualTo(300));
            Assert.That(bank.Egyenleg("2222"), Is.EqualTo(200));
            Assert.That(bank.Egyenleg("3333"), Is.EqualTo(700));
        }
    }
}