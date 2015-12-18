using System;

namespace UniPromise {
	internal interface IUniPromiseManager {
		void AddCallback(Action callback);
	}
}
