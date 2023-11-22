using EdjCase.ICP.Candid.Mapping;
using CanisterPK.CanisterMatchMaking.Models;
using System;

namespace CanisterPK.CanisterMatchMaking.Models
{
	[Variant(typeof(CanisterWsMessageResultTag))]
	public class CanisterWsMessageResult
	{
		[VariantTagProperty()]
		public CanisterWsMessageResultTag Tag { get; set; }

		[VariantValueProperty()]
		public object? Value { get; set; }

		public CanisterWsMessageResult(CanisterWsMessageResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected CanisterWsMessageResult()
		{
		}

		public static CanisterWsMessageResult Err(string info)
		{
			return new CanisterWsMessageResult(CanisterWsMessageResultTag.Err, info);
		}

		public static CanisterWsMessageResult Ok()
		{
			return new CanisterWsMessageResult(CanisterWsMessageResultTag.Ok, null);
		}

		public string AsErr()
		{
			this.ValidateTag(CanisterWsMessageResultTag.Err);
			return (string)this.Value!;
		}

		private void ValidateTag(CanisterWsMessageResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum CanisterWsMessageResultTag
	{
		[VariantOptionType(typeof(string))]
		Err,
		Ok
	}
}