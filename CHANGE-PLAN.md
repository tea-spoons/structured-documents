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
<!-- review-items:start -->
- [ ] **P1** Restore Markdown as an optional integration with Markdig (existing item, made concrete): document how to add it (NuGetForUnity or a vendored DLL), gate the code with a define, and verify it in an IL2CPP player build (not checked yet).
- [ ] **P1** Replace the plain `Exception` with a `DocumentException` that carries the path, and test the collision case.
- [ ] **P1** Declares `unity: 2022.3`, but only Unity 6000.3.8f1 was tested. Add a Unity version matrix to CI once package tests run there (see the `unity-ci-kit` plan), or raise the minimum.
- [ ] **P2** Add parity tests so JSON, XML and Markdown documents give the same values for the same data.
- [ ] **P2** Add a `CHANGELOG.md`. Unity's package layout lists one next to `README.md`, and the `unity-ci-kit` validator warns without it.
<!-- review-items:end -->

<!-- review:start -->
## Review (September 2026)

Reviewed as a senior Unity engineer would: I read the code and compared the package with similar open-source projects (September 2026). Those projects are listed for ideas only. Nothing was copied from them, and their licenses are noted in case code is ever reused. Priorities: **P0** correctness bug or broken metadata, **P1** should be done soon, **P2** nice to have.

### Compared with

| Project | License | Worth noting |
|---|---|---|
| [xoofx/markdig](https://github.com/xoofx/markdig) | BSD-2-Clause | CommonMark-compliant, extensible Markdown processor for .NET with a fast parser and low GC pressure. It is on NuGet, not a UPM package. |

### Findings from reading the code

- **[Feature]** Markdown support is compiled out. `StructuredMarkdownDocument` exists but needs a Markdown parser that is not in the project.
- **[Errors]** `StructuredMarkdownDocument` throws a plain `Exception("Collision between equal heading paths.")` and has a TODO to create a custom exception class.
- **[Tests]** 2 test files, 146 lines.
<!-- review:end -->

## Notes and ideas

_Add your own here._
