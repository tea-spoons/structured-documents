# Change plan

> Draft. This file tracks what changed on the way to this repo and what I plan to change next. Edit freely.

## Origin

Originally developed at Bigpoint. Published here with Bigpoint's permission for research, education and other noncommercial use. Copyright (c) 2026 Bigpoint; see [LICENSE.md](LICENSE.md).

## Changes made before publishing

- Namespaces are now `TeaSpoons.*` and the package id is `com.tea-spoons.structured-documents` (assemblies renamed to match).
- Internal build, registry and tracker references were removed; the repo uses GitHub Actions (`CI` and `Release`) built on `unity-ci-kit`.
- Added `LICENSE.md` (PolyForm Noncommercial 1.0.0), an install section in the README, and package metadata (author, license and documentation URLs).
- Markdown support is disabled: it needed a Markdig integration that is not public. The `MARKDIG` code paths stay compiled out; JSON is unchanged.
- 0.7.0: `com.tea-spoons.collections` is no longer a dependency. `PreloadingStructuredDocument.data` uses its `ListDictionary` when the project has it (`TEASPOONS_COLLECTIONS`, set from the asmdef `versionDefines`) and otherwise a small nested, protected stand-in with the same members. Added tests for missing paths, out-of-range indices and `Clear`. Structured Documents now has no dependencies.

## Planned changes

- [x] Tag and publish `v0.6.1` with the Release workflow.
- [x] Tag and publish `v0.7.0` with the Release workflow.
- [ ] Run this package's tests in CI with `unity-ci-kit` (needs a small test-project helper in the kit).
- [ ] Restore Markdown support as an optional integration with the public Markdig library.

## Notes and ideas

_Add your own here._
