﻿#if SIGTRAP_RELAY_TEST && UNITY_5_3_OR_NEWER
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Sigtrap.Relays.Link;

namespace Sigtrap.Relays.Tests {
	public class RelayTests {
		int calls;

		// Only used for self remove/add tests where cached ref needed
		Relay myRelay;
		Relay<int> myRelay1;
		Relay<int,float> myRelay2;
		Relay<int,float,bool> myRelay3;
		Relay<int,float,bool,uint> myRelay4;

		#region Lifecycle
		[OneTimeSetUp]
		public void PreFixture(){
			
		}
		[OneTimeTearDown]
		public void PostFixture(){

		}

		[SetUp]
		public void PreTest(){
			calls = 0;
		}
		[TearDown]
		public void PostTest(){
			
		}
		#endregion

		#region Tests
		#region Addition
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestAddition(int args){
			var r = CreateRelay(args);
			AddListener1(r,args);
			AssertListeners(r, args, 1, 0);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestNoDupAddition(int args){
			var r = CreateRelay(args);
			AddListener1(r,args);
			AddListener1(r,args);
			AssertListeners(r, args, 1, 0);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestDupAddition(int args){
			var r = CreateRelay(args);
			AddListener1(r,args);
			AddListener1(r,args,true);
			AssertListeners(r, args, 2, 0);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestMultipleAddition(int args){
			var r = CreateRelay(args);
			AddListener1(r,args);
			AddListener2(r,args);
			AssertListeners(r, args, 2, 0);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestOnceAddition(int args){
			var r = CreateRelay(args);
			AddListenerOnce1(r,args);
			AssertListeners(r, args, 0, 1);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestOnceNoDupAddition(int args){
			var r = CreateRelay(args);
			AddListenerOnce1(r,args);
			AddListenerOnce1(r,args);
			AssertListeners(r, args, 0, 1);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestOnceDupAddition(int args){
			var r = CreateRelay(args);
			AddListenerOnce1(r,args);
			AddListenerOnce1(r,args, true);
			AssertListeners(r, args, 0, 2);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestOnceMultipleAddition(int args){
			var r = CreateRelay(args);
			AddListenerOnce1(r,args);
			AddListenerOnce2(r,args);
			AssertListeners(r, args, 0, 2);
		}
		#endregion

		#region Call
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestCall(int args){
			var r = CreateRelay(args);
			AddListener1(r,args);
			Dispatch(r,args);
			Assert.That(calls == 1);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestDupCalls(int args){
			var r = CreateRelay(args);
			AddListener1(r,args);
			AddListener1(r,args,true);
			Dispatch(r,args);
			Assert.That(calls == 2);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestMultipleCalls(int args){
			var r = CreateRelay(args);
			AddListener1(r,args);
			AddListener2(r,args);
			Dispatch(r,args);
			Assert.That(calls == 2);
		}

		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestCallOnce(int args){
			var r = CreateRelay(args);
			AddListenerOnce1(r,args);
			Dispatch(r,args);
			Assert.That(calls == 1);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestDupCallsOnce(int args){
			var r = CreateRelay(args);
			AddListenerOnce1(r,args);
			AddListenerOnce1(r,args,true);
			Dispatch(r,args);
			Assert.That(calls == 2);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestMultipleCallsOnce(int args){
			var r = CreateRelay(args);
			AddListenerOnce1(r,args);
			AddListenerOnce2(r,args);
			Dispatch(r,args);
			Assert.That(calls == 2);
		}
		#endregion

		#region Removal
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestRemCalls(int args){
			var r = CreateRelay(args);
			AddListener1(r,args);
			AddListener2(r,args);
			RemListener1(r,args);
			RemListener2(r,args);
			AssertListeners(r, args, 0, 0);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestCallOnceRemoval(int args){
			var r = CreateRelay(args);
			AddListenerOnce1(r,args);
			Dispatch(r,args);
			AssertListeners(r, args, 0, 0);
			Dispatch(r,args);
			Assert.That(calls == 1);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestDupCallsOnceRemoval(int args){
			var r = CreateRelay(args);
			AddListenerOnce1(r,args);
			AddListenerOnce1(r,args,true);
			Dispatch(r,args);
			AssertListeners(r, args, 0, 0);
			Dispatch(r,args);
			Assert.That(calls == 2);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestMultipleCallsOnceRemoval(int args){
			var r = CreateRelay(args);
			AddListenerOnce1(r,args);
			AddListenerOnce2(r,args);
			Dispatch(r,args);
			AssertListeners(r, args, 0, 0);
			Dispatch(r,args);
			Assert.That(calls == 2);
		}
		#endregion

		#region Clear
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestClear(int args){
			var r = CreateRelay(args);
			AddListener1(r,args);
			AddListener2(r,args);
			ClearListeners(r,args,true,false);
			AssertListeners(r, args, 0, 0);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestClearOnce(int args){
			var r = CreateRelay(args);
			AddListenerOnce1(r,args);
			AddListenerOnce2(r,args);
			ClearListeners(r,args,false,true);
			AssertListeners(r, args, 0, 0);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestClearAll(int args){
			var r = CreateRelay(args);
			AddListener1(r,args);
			AddListener2(r,args);
			AddListenerOnce1(r,args);
			AddListenerOnce2(r,args);
			ClearListeners(r,args,true,true);
			AssertListeners(r, args, 0, 0);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestClearPersistentOnly(int args){
			var r = CreateRelay(args);
			AddListener1(r,args);
			AddListener2(r,args);
			AddListenerOnce1(r,args);
			AddListenerOnce2(r,args);
			ClearListeners(r,args,true,false);
			AssertListeners(r, args, 0, 2);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestClearOnceOnly(int args){
			var r = CreateRelay(args);
			AddListener1(r,args);
			AddListener2(r,args);
			AddListenerOnce1(r,args);
			AddListenerOnce2(r,args);
			ClearListeners(r,args,false,true);
			AssertListeners(r, args, 2, 0);
		}
		#endregion

		#region Self-Removal
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestSelfRemoval(int args){
			object r = CreateSelfRemovalRelay(args, false);
			Dispatch(r,args);
			Assert.That(calls == 1);
			AssertListeners(r, args, 0, 0);
			Dispatch(r,args);
			Assert.That(calls == 1);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestSelfRemovalWithMultipleListeners(int args){
			object r = CreateSelfRemovalRelay(args, true);
			AddListener2(r,args);
			Dispatch(r,args);
			Assert.That(calls == 3);
			AssertListeners(r, args, 2, 0);
			Dispatch(r,args);
			Assert.That(calls == 5);
		}

		object CreateSelfRemovalRelay(int args, bool addListenerBefore){
			object r = null;
			switch (args){
				case 0:
					myRelay = new Relay();
					if (addListenerBefore) myRelay.AddListener(DummyDelegate1);
					myRelay.AddListener(SelfRemDelegate);
					r = myRelay;
					break;
				case 1:
					myRelay1 = new Relay<int>();
					if (addListenerBefore) myRelay1.AddListener(DummyDelegate1);
					myRelay1.AddListener(SelfRemDelegate);
					r = myRelay1;
					break;
				case 2:
					myRelay2 = new Relay<int, float>();
					if (addListenerBefore) myRelay2.AddListener(DummyDelegate1);
					myRelay2.AddListener(SelfRemDelegate);
					r = myRelay2;
					break;
				case 3:
					myRelay3 = new Relay<int, float, bool>();
					if (addListenerBefore) myRelay3.AddListener(DummyDelegate1);
					myRelay3.AddListener(SelfRemDelegate);
					r = myRelay3;
					break;
				case 4:
					myRelay4 = new Relay<int, float, bool, uint>();
					if (addListenerBefore) myRelay4.AddListener(DummyDelegate1);
					myRelay4.AddListener(SelfRemDelegate);
					r = myRelay4;
					break;
			}
			return r;
		}
		#endregion

		#region Syncronous Addition
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestSyncAdd(int args){
			object r = CreateSyncAddRelay(args);
			Dispatch(r,args);
			Assert.That(calls == 1);
			Dispatch(r,args);
			Assert.That(calls == 3);
		}

		object CreateSyncAddRelay(int args){
			object r = null;
			switch (args){
				case 0:
					myRelay = new Relay();
					myRelay.AddListener(SyncAddDelegate);
					r = myRelay;
					break;
				case 1:
					myRelay1 = new Relay<int>();
					myRelay1.AddListener(SyncAddDelegate);
					r = myRelay1;
					break;
				case 2:
					myRelay2 = new Relay<int, float>();
					myRelay2.AddListener(SyncAddDelegate);
					r = myRelay2;
					break;
				case 3:
					myRelay3 = new Relay<int, float, bool>();
					myRelay3.AddListener(SyncAddDelegate);
					r = myRelay3;
					break;
				case 4:
					myRelay4 = new Relay<int, float, bool, uint>();
					myRelay4.AddListener(SyncAddDelegate);
					r = myRelay4;
					break;
			}
			return r;
		}
		#endregion

		#region Syncronous Addition And Self-Removal
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestSyncAddAndSelfRemoval(int args){
			object r = CreateSyncAddSelfRemRelay(args, false);
			Dispatch(r,args);
			Assert.That(calls == 1);
			Dispatch(r,args);
			Assert.That(calls == 2);
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestSyncAddAndSelfRemovalWithMultipleListeners(int args){
			object r = CreateSyncAddSelfRemRelay(args, true);
			Dispatch(r,args);
			Assert.That(calls == 2);
			Dispatch(r,args);
			Assert.That(calls == 4);
		}

		object CreateSyncAddSelfRemRelay(int args, bool addListenerBefore){
			object r = null;
			switch (args){
				case 0:
					myRelay = new Relay();
					if (addListenerBefore) myRelay.AddListener(DummyDelegate1);
					myRelay.AddListener(SyncAddSelfRemDelegate);
					r = myRelay;
					break;
				case 1:
					myRelay1 = new Relay<int>();
					if (addListenerBefore) myRelay1.AddListener(DummyDelegate1);
					myRelay1.AddListener(SyncAddSelfRemDelegate);
					r = myRelay1;
					break;
				case 2:
					myRelay2 = new Relay<int, float>();
					if (addListenerBefore) myRelay2.AddListener(DummyDelegate1);
					myRelay2.AddListener(SyncAddSelfRemDelegate);
					r = myRelay2;
					break;
				case 3:
					myRelay3 = new Relay<int, float, bool>();
					if (addListenerBefore) myRelay3.AddListener(DummyDelegate1);
					myRelay3.AddListener(SyncAddSelfRemDelegate);
					r = myRelay3;
					break;
				case 4:
					myRelay4 = new Relay<int, float, bool, uint>();
					if (addListenerBefore) myRelay4.AddListener(DummyDelegate1);
					myRelay4.AddListener(SyncAddSelfRemDelegate);
					r = myRelay4;
					break;
			}
			return r;
		}
		#endregion

		#region RelayLink
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestLinkLazyInstantiation(int args){
			var r = CreateRelay(args);
			switch (args){
				case 0:
					{
						var link = (r as Relay).link;
						Assert.That(link != null && link == (r as Relay).link);
					}
					break;
				case 1:
					{
						var link = (r as Relay<int>).link;
						Assert.That(link != null && link == (r as Relay<int>).link);
					}
					break;
				case 2:
					{
						var link = (r as Relay<int,float>).link;
						Assert.That(link != null && link == (r as Relay<int,float>).link);
					}
					break;
				case 3:
					{
						var link = (r as Relay<int,float,bool>).link;
						Assert.That(link != null && link == (r as Relay<int,float,bool>).link);
					}
					break;
				case 4:
					{
						var link = (r as Relay<int,float,bool,uint>).link;
						Assert.That(link != null && link == (r as Relay<int,float,bool,uint>).link);
					}
					break;
			}
		}
		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void TestLinkAddAndCall(int args){
			var r = CreateRelay(args);
			AddListenerByLink(r,args, true);
			AddListenerByLink(r,args, true);
			AssertListeners(r, args, 2, 0);
			Dispatch(r,args);
			Assert.That(calls == 2);
		}
		#endregion
		#endregion

		#region Helpers
		// Messy as hell but necessary since no way to automatically deal with variable number of type args
		void Dispatch(object r, int args){
			switch (args){
				case 0:
					(r as Relay).Dispatch();
					break;
				case 1:
					(r as Relay<int>).Dispatch(1);
					break;
				case 2:
					(r as Relay<int, float>).Dispatch(1, 2f);
					break;
				case 3:
					(r as Relay<int, float, bool>).Dispatch(1, 2f, true);
					break;
				case 4:
					(r as Relay<int, float, bool, uint>).Dispatch(1, 2f, true, 3);
					break;
			}
		}
		void AddListenerByLink(object r, int args, bool allowDups){
			switch (args){
				case 0:
					(r as Relay).link.AddListener(DummyDelegate1,allowDups);
					break;
				case 1:
					(r as Relay<int>).link.AddListener(DummyDelegate1,allowDups);
					break;
				case 2:
					(r as Relay<int, float>).link.AddListener(DummyDelegate1,allowDups);
					break;
				case 3:
					(r as Relay<int, float, bool>).link.AddListener(DummyDelegate1,allowDups);
					break;
				case 4:
					(r as Relay<int, float, bool, uint>).link.AddListener(DummyDelegate1,allowDups);
					break;
			}
		}
		object CreateRelay(int args){
			switch (args){
				case 0:
					return new Relay();
				case 1:
					return new Relay<int>();
				case 2:
					return new Relay<int,float>();
				case 3:
					return new Relay<int,float,bool>();
				case 4:
					return new Relay<int,float,bool,uint>();
			}
			return null;
		}
		void AddListener1(object r, int args, bool allowDups=false){
			switch (args){
				case 0:
					(r as Relay).AddListener(DummyDelegate1,allowDups);
					break;
				case 1:
					(r as Relay<int>).AddListener(DummyDelegate1,allowDups);
					break;
				case 2:
					(r as Relay<int, float>).AddListener(DummyDelegate1,allowDups);
					break;
				case 3:
					(r as Relay<int, float, bool>).AddListener(DummyDelegate1,allowDups);
					break;
				case 4:
					(r as Relay<int, float, bool, uint>).AddListener(DummyDelegate1,allowDups);
					break;
			}
		}
		void AddListener2(object r, int args, bool allowDups=false){
			switch (args){
				case 0:
					(r as Relay).AddListener(DummyDelegate2,allowDups);
					break;
				case 1:
					(r as Relay<int>).AddListener(DummyDelegate2,allowDups);
					break;
				case 2:
					(r as Relay<int, float>).AddListener(DummyDelegate2,allowDups);
					break;
				case 3:
					(r as Relay<int, float, bool>).AddListener(DummyDelegate2,allowDups);
					break;
				case 4:
					(r as Relay<int, float, bool, uint>).AddListener(DummyDelegate2,allowDups);
					break;
			}
		}
		void AddListenerOnce1(object r, int args, bool allowDups=false){
			switch (args){
				case 0:
					(r as Relay).AddOnce(DummyDelegate1,allowDups);
					break;
				case 1:
					(r as Relay<int>).AddOnce(DummyDelegate1,allowDups);
					break;
				case 2:
					(r as Relay<int, float>).AddOnce(DummyDelegate1,allowDups);
					break;
				case 3:
					(r as Relay<int, float, bool>).AddOnce(DummyDelegate1,allowDups);
					break;
				case 4:
					(r as Relay<int, float, bool, uint>).AddOnce(DummyDelegate1,allowDups);
					break;
			}
		}
		void AddListenerOnce2(object r, int args, bool allowDups=false){
			switch (args){
				case 0:
					(r as Relay).AddOnce(DummyDelegate2,allowDups);
					break;
				case 1:
					(r as Relay<int>).AddOnce(DummyDelegate2,allowDups);
					break;
				case 2:
					(r as Relay<int, float>).AddOnce(DummyDelegate2,allowDups);
					break;
				case 3:
					(r as Relay<int, float, bool>).AddOnce(DummyDelegate2,allowDups);
					break;
				case 4:
					(r as Relay<int, float, bool, uint>).AddOnce(DummyDelegate2,allowDups);
					break;
			}
		}
		void RemListener1(object r, int args){
			switch (args){
				case 0:
					(r as Relay).RemoveListener(DummyDelegate1);
					break;
				case 1:
					(r as Relay<int>).RemoveListener(DummyDelegate1);
					break;
				case 2:
					(r as Relay<int, float>).RemoveListener(DummyDelegate1);
					break;
				case 3:
					(r as Relay<int, float, bool>).RemoveListener(DummyDelegate1);
					break;
				case 4:
					(r as Relay<int, float, bool, uint>).RemoveListener(DummyDelegate1);
					break;
			}
		}
		void RemListener2(object r, int args){
			switch (args){
				case 0:
					(r as Relay).RemoveListener(DummyDelegate2);
					break;
				case 1:
					(r as Relay<int>).RemoveListener(DummyDelegate2);
					break;
				case 2:
					(r as Relay<int, float>).RemoveListener(DummyDelegate2);
					break;
				case 3:
					(r as Relay<int, float, bool>).RemoveListener(DummyDelegate2);
					break;
				case 4:
					(r as Relay<int, float, bool, uint>).RemoveListener(DummyDelegate2);
					break;
			}
		}
		void ClearListeners(object r, int args, bool persistent, bool once){
			switch (args){
				case 0:
					(r as Relay).RemoveAll(persistent, once);
					break;
				case 1:
					(r as Relay<int>).RemoveAll(persistent, once);
					break;
				case 2:
					(r as Relay<int, float>).RemoveAll(persistent, once);
					break;
				case 3:
					(r as Relay<int, float, bool>).RemoveAll(persistent, once);
					break;
				case 4:
					(r as Relay<int, float, bool, uint>).RemoveAll(persistent, once);
					break;
			}
		}

		uint GetListeners(object r, int args){
			switch (args){
				case 0:
					return (r as Relay).listenerCount;
				case 1:
					return (r as Relay<int>).listenerCount;
				case 2:
					return (r as Relay<int, float>).listenerCount;
				case 3:
					return (r as Relay<int, float, bool>).listenerCount;
				case 4:
					return (r as Relay<int, float, bool, uint>).listenerCount;
			}
			return 999;
		}
		uint GetListenersOnce(object r, int args){
			switch (args){
				case 0:
					return (r as Relay).oneTimeListenersCount;
				case 1:
					return (r as Relay<int>).oneTimeListenersCount;
				case 2:
					return (r as Relay<int, float>).oneTimeListenersCount;
				case 3:
					return (r as Relay<int, float, bool>).oneTimeListenersCount;
				case 4:
					return (r as Relay<int, float, bool, uint>).oneTimeListenersCount;
			}
			return 999;
		}
		void AssertListeners(object r, int args, int persistent, int onetime){
			Assert.That((int)GetListeners(r,args) == persistent && (int)GetListenersOnce(r,args) == onetime);
		}
		#endregion

		#region Delegates
		void DummyDelegate1(){
			++calls;
		}
		void DummyDelegate1(int i){
			DummyDelegate1();
		}
		void DummyDelegate1(int i, float f){
			DummyDelegate1();
		}
		void DummyDelegate1(int i, float f, bool b){
			DummyDelegate1();
		}
		void DummyDelegate1(int i, float f, bool b, uint u){
			DummyDelegate1();
		}

		void DummyDelegate2(){
			DummyDelegate1();
		}
		void DummyDelegate2(int i){
			DummyDelegate2();
		}
		void DummyDelegate2(int i, float f){
			DummyDelegate2();
		}
		void DummyDelegate2(int i, float f, bool b){
			DummyDelegate2();
		}
		void DummyDelegate2(int i, float f, bool b, uint u){
			DummyDelegate2();
		}

		void SelfRemDelegate(){
			DummyDelegate1();
			myRelay.RemoveListener(SelfRemDelegate);
		}
		void SelfRemDelegate(int i){
			DummyDelegate1();
			myRelay1.RemoveListener(SelfRemDelegate);
		}
		void SelfRemDelegate(int i, float f){
			DummyDelegate1();
			myRelay2.RemoveListener(SelfRemDelegate);
		}
		void SelfRemDelegate(int i, float f, bool b){
			DummyDelegate1();
			myRelay3.RemoveListener(SelfRemDelegate);
		}
		void SelfRemDelegate(int i, float f, bool b, uint u){
			DummyDelegate1();
			myRelay4.RemoveListener(SelfRemDelegate);
		}

		void SyncAddDelegate(){
			DummyDelegate1();
			myRelay.AddListener(DummyDelegate2);
		}
		void SyncAddDelegate(int i){
			DummyDelegate1();
			myRelay1.AddListener(DummyDelegate2);
		}
		void SyncAddDelegate(int i, float f){
			DummyDelegate1();
			myRelay2.AddListener(DummyDelegate2);
		}
		void SyncAddDelegate(int i, float f, bool b){
			DummyDelegate1();
			myRelay3.AddListener(DummyDelegate2);
		}
		void SyncAddDelegate(int i, float f, bool b, uint u){
			DummyDelegate1();
			myRelay4.AddListener(DummyDelegate2);
		}

		void SyncAddSelfRemDelegate(){
			DummyDelegate1();
			myRelay.AddListener(DummyDelegate2);
			myRelay.RemoveListener(SyncAddSelfRemDelegate);
		}
		void SyncAddSelfRemDelegate(int i){
			DummyDelegate1();
			myRelay1.AddListener(DummyDelegate2);
			myRelay1.RemoveListener(SyncAddSelfRemDelegate);
		}
		void SyncAddSelfRemDelegate(int i, float f){
			DummyDelegate1();
			myRelay2.AddListener(DummyDelegate2);
			myRelay2.RemoveListener(SyncAddSelfRemDelegate);
		}
		void SyncAddSelfRemDelegate(int i, float f, bool b){
			DummyDelegate1();
			myRelay3.AddListener(DummyDelegate2);
			myRelay3.RemoveListener(SyncAddSelfRemDelegate);
		}
		void SyncAddSelfRemDelegate(int i, float f, bool b, uint u){
			DummyDelegate1();
			myRelay4.AddListener(DummyDelegate2);
			myRelay4.RemoveListener(SyncAddSelfRemDelegate);
		}
		#endregion
	}
}
#endif