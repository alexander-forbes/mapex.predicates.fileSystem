using Moq;
using Notus;
using NUnit.Framework;

namespace Mapex.Predicates.FileSystem.Tests
{
	[TestFixture]
	public class When_calling_matches_on_file_system_where_specification
	{
		[Test]
		public void It_should_return_false_when_the_metadata_property_is_null()
		{
			var document = new Mock<IDocument>();
			var specification = new FileSystemWhereSpecification();

			Assert.IsFalse(specification.Matches(document.Object));
		}

		[Test]
		public void It_should_return_false_when_the_metadata_does_not_contain_a_filename_key()
		{
			var document = new Mock<IDocument>();
			document.Setup(d => d.Metadata).Returns(new Metadata());

			var specification = new FileSystemWhereSpecification();

			Assert.IsFalse(specification.Matches(document.Object));
		}

		[Test]
		public void It_should_return_true_when_the_metadata_filename_value_matches_the_filename_property_value()
		{
			var document = new Mock<IDocument>();

			document.Setup(d => d.Metadata).Returns(new Metadata
			{
				{"Filename", "PDDownload20180328.xls"}
			});

			var specification = new FileSystemWhereSpecification
			{
				Filename = @"^PDDownload[\d]+.xls$"
			};

			Assert.IsTrue(specification.Matches(document.Object));
		}

		[Test]
		public void It_should_return_false_when_the_metadata_filename_value_does_not_match_the_filename_property_value()
		{
			var document = new Mock<IDocument>();

			document.Setup(d => d.Metadata).Returns(new Metadata
			{
				{"Filename", "processed-PDDownload20180328.xls"}
			});

			var specification = new FileSystemWhereSpecification
			{
				Filename = @"^PDDownload[\d]+.xls$"
			};

			Assert.IsFalse(specification.Matches(document.Object));
		}

		[Test]
		public void It_should_return_true_when_the_metadata_path_value_matches_the_path_property_value()
		{
			var document = new Mock<IDocument>();

			document.Setup(d => d.Metadata).Returns(new Metadata
			{
				{"Filename", "PDDownload20180328.xls"},
				{"Path", @"C:\temp" }
			});

			var specification = new FileSystemWhereSpecification
			{
				Filename = @"^PDDownload[\d]+.xls$",
				Path = @"C:\\temp"
			};

			Assert.IsTrue(specification.Matches(document.Object));
		}

		[Test]
		public void It_should_return_false_when_the_metadata_path_value_does_not_match_the_path_property_value()
		{
			var document = new Mock<IDocument>();

			document.Setup(d => d.Metadata).Returns(new Metadata
			{
				{"Filename", "PDDownload20180328.xls"},
				{"Path", @"C:\temp" }
			});

			var specification = new FileSystemWhereSpecification
			{
				Filename = @"^PDDownload[\d]+.xls$",
				Path = @"C:\\other"
			};

			Assert.IsFalse(specification.Matches(document.Object));
		}

		[Test]
		public void It_should_return_true_when_the_metadata_extension_value_matches_the_extension_property_value()
		{
			var document = new Mock<IDocument>();

			document.Setup(d => d.Metadata).Returns(new Metadata
			{
				{"Filename", "PDDownload20180328.xls"},
				{"Extension", ".xls" }
			});

			var specification = new FileSystemWhereSpecification
			{
				Filename = @"^PDDownload[\d]+.xls$",
				Extension = "^.xls$"
			};

			Assert.IsTrue(specification.Matches(document.Object));
		}

		[Test]
		public void It_should_return_false_when_the_metadata_extension_value_does_not_match_the_extension_property_value()
		{
			var document = new Mock<IDocument>();

			document.Setup(d => d.Metadata).Returns(new Metadata
			{
				{"Filename", "PDDownload20180328.xls"},
				{"Extension", ".xls" }
			});

			var specification = new FileSystemWhereSpecification
			{
				Filename = @"^PDDownload[\d]+.xls$",
				Extension = "^.txt$"
			};

			Assert.IsFalse(specification.Matches(document.Object));
		}

		[TestFixture]
		public class When_calling_validate_on_file_system_where_specification
		{
			[Test]
			public void It_should_return_a_notification_error_when_no_filename_value_is_specified()
			{
				var specification = new FileSystemWhereSpecification();
				var notification = new Notification();

				specification.Validate(notification);

				Assert.IsTrue(notification.IncludesError("Filename value has not been specified."));
			}
		}
	}
}
