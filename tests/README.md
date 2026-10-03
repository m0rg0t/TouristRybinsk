# Portable source regressions

## Fix
Four Group-5 photo IDs incorrectly used the Group-4 prefix, including a collision that made GetItem return null for both photos. The Group-3 description and two Group-8 photos also referenced another group. Correct only those identities and group references; keep all 26 items, eight groups, text and asset paths.

## Run
With .NET SDK 10.0.401 installed:

    dotnet run --project tests/Portable.Tests/Portable.Tests.csproj --configuration Release

This is a dependency-free net10.0 console harness with C# 5 syntax checking. Its project links the actual production files, without copying their logic. NuGet sources are cleared. The SDK runs the tests only; the historical application is not retargeted.

## Coverage
Real catalog construction and exact-object lookup for every item, item/group identity alignment and unknown IDs.

## Original failure
The same tests were run against original source at [2c4c8445c4cc068378a650db146cdd990003d5e9](https://github.com/m0rg0t/TouristRybinsk/tree/2c4c8445c4cc068378a650db146cdd990003d5e9) and exited nonzero; the repaired source exits zero.

    FAIL: group ownership: Group-3-Item-1: Assertion failed
    FAIL: exact item lookup: Group-4-Item-2: Assertion failed
    FAIL: exact item lookup: Group-4-Item-2: Assertion failed
    FAIL: group ownership: Group-4-Item-2: Assertion failed
    FAIL: group ownership: Group-4-Item-3: Assertion failed
    FAIL: group ownership: Group-4-Item-4: Assertion failed
    FAIL: group ownership: Group-4-Item-5: Assertion failed
    FAIL: group ownership: Group-8-Item-2: Assertion failed
    FAIL: group ownership: Group-8-Item-3: Assertion failed

## Boundaries
The stand-ins in Portable.Tests/Stubs.cs only supply unavailable platform types; they do not prove platform compatibility or simulate the app UI. No real service/user data, credentials, notifications, payments, publishing or deployment are used.

Previously saved Group-5 photo IDs cannot always be distinguished from Group-4 IDs; ambiguous old selections may not restore. No asset or application-target changes.

The full historical Windows/Phone build and device runtime remain unverified. Original project targets, package manifests, vendor binaries and assets are unchanged. No compatible modern replacement for the old platform-specific dependencies has been established, so no speculative package upgrade is included.
