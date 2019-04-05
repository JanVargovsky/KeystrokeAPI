// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include <iostream>
#include <thread>

BOOL APIENTRY DllMain(
	HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved
)
{
	std::cout << "hello injected world ";
	auto tid = std::this_thread::get_id();
	auto pid = _getpid();

	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		std::cout << "process attach";
		break;
	case DLL_PROCESS_DETACH:
		std::cout << "process detach";
		break;
	case DLL_THREAD_ATTACH:
		std::cout << "thread attach";
		break;
	case DLL_THREAD_DETACH:
		std::cout << "thread detach";
		break;
	}
	std::cout << " pid=" << pid << " tid=" << tid << std::endl;
	return TRUE;
}

