"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const express_1 = __importDefault(require("express"));
const http_proxy_middleware_1 = require("http-proxy-middleware");
const morgan_1 = __importDefault(require("morgan"));
const cors_1 = __importDefault(require("cors"));
const app = (0, express_1.default)();
const port = 3000;
let fallback = { type: "NOT_FOUND", statusCode: 404 };
let router = undefined;
function setupRouter(endpoints) {
    router = express_1.default.Router();
    router.use((0, morgan_1.default)("combined"));
    router.use((0, cors_1.default)());
    router.use(express_1.default.json());
    router.get("/healthz", (req, res) => {
        res.send();
    });
    router.get("/_admin/endpoints", (req, res) => {
        res.send(endpoints);
    });
    router.post("/_admin/endpoints", (req, res) => {
        const endpoint = req.body;
        const newEndpoints = [...endpoints, endpoint];
        setupRouter(newEndpoints);
        res.status(200);
        res.send(newEndpoints);
    });
    router.get("/_admin/fallback", (req, res) => {
        res.send(fallback);
    });
    router.patch("/_admin/fallback", (req, res) => {
        console.log("received", req.body);
        const newFallback = req.body; // TODO: validate
        fallback = newFallback;
        res.send();
    });
    for (const endpoint of endpoints) {
        router.use(endpoint.path, (req, res, next) => {
            if (req.method !== endpoint.method) {
                next();
                return;
            }
            res.status(endpoint.statusCode);
            res.send(endpoint.body);
        });
    }
    router.use((req, res, next) => {
        if (fallback.type === "JSON") {
            res.status(fallback.statusCode);
            res.send(fallback.json);
            next();
            return;
        }
        if (fallback.type === "REDIRECT") {
            (0, http_proxy_middleware_1.createProxyMiddleware)({ target: fallback.baseUrl, changeOrigin: true })(req, res, next);
            return;
        }
        res.status(404);
        res.send();
        next();
    });
}
app.use((req, res, next) => {
    if (!router)
        throw new Error("router has not been initialized");
    router(req, res, next);
});
setupRouter([]);
app.listen(port, () => {
    console.log(`Server listening on port ${port}`);
});
