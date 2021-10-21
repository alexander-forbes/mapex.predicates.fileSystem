using System;
using System.Text.RegularExpressions;
using Mapex.Specifications;
using Notus;

namespace Mapex.Predicates.FileSystem
{
	public class FileSystemWhereSpecification : IWhereSpecification
	{
		public string Path { get; set; }
		public string Filename { get; set; }
		public string Extension { get; set; }

		public bool Matches(IDocument document)
		{
			if (document == null)
				throw new ArgumentNullException(nameof(document));

			return MatchFilename(document) &&
				   MatchPath(document) &&
				   MatchExtension(document);
		}

		public void Validate(Notification notification)
		{
			if (string.IsNullOrEmpty(Filename))
				notification.AddError("Filename value has not been specified.");
		}

		private bool MatchFilename(IDocument document)
		{
			return document.Metadata != null &&
				document.Metadata.ContainsKey("Filename") &&
				new Regex(Filename).IsMatch(document.Metadata["Filename"]);
		}

		private bool MatchPath(IDocument document)
		{
			if (string.IsNullOrEmpty(Path)) // Path not specified, but it is optional
				return true;

			return document.Metadata.ContainsKey("Path") && new Regex(Path).IsMatch(document.Metadata["Path"]);
		}

		private bool MatchExtension(IDocument document)
		{
			if (string.IsNullOrEmpty(Extension)) // Extension not specified, but it is optional
				return true;

			return document.Metadata.ContainsKey("Extension") && new Regex(Extension).IsMatch(document.Metadata["Extension"]);
		}
	}
}
