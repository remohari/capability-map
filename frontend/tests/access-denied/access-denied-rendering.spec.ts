// Intended shared access-denied scenarios.
//
// 1. The access denied page renders the reason code from query params.
// 2. A blocked route guard redirects to /access-denied with target metadata.
// 3. User-facing denial feedback remains visible instead of failing silently.
