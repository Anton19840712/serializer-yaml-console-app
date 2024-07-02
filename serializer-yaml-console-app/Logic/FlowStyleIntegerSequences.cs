using YamlDotNet.Core.Events;
using YamlDotNet.Core;
using YamlDotNet.Serialization.EventEmitters;
using YamlDotNet.Serialization;

namespace serializer_yaml_console_app.Logic
{
	class FlowStyleIntegerSequences : ChainedEventEmitter
	{
		public FlowStyleIntegerSequences(IEventEmitter nextEmitter)
			: base(nextEmitter) { }

		public override void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
		{
			if (typeof(IEnumerable<string>).IsAssignableFrom(eventInfo.Source.Type))
			{
				eventInfo = new SequenceStartEventInfo(eventInfo.Source)
				{
					Style = SequenceStyle.Flow
				};
			}

			nextEmitter.Emit(eventInfo, emitter);
		}
	}
}