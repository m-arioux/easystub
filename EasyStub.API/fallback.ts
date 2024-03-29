import { Router } from "express";
import { createProxyMiddleware } from "http-proxy-middleware";

export type Fallback = NotFoundFallback | RedirectFallback | JsonFallback;

export interface NotFoundFallback {
  type: "NOT_FOUND";
}

export interface RedirectFallback {
  type: "REDIRECT";
  baseUrl: string;
}

export interface JsonFallback {
  type: "JSON";
  statusCode: number;
  json?: any;
}

let fallback: Fallback = { type: "NOT_FOUND" };

export function setupFallback(router: Router) {
  router.get("/_admin/fallback", (req, res) => {
    res.send(fallback);
  });

  router.patch("/_admin/fallback", (req, res) => {
    console.log("received", req.body);
    const newFallback = req.body; // TODO: validate

    fallback = newFallback;

    res.send();
  });
}

export function useFallback(router: Router) {
  router.use((req, res, next) => {
    if (fallback.type === "JSON") {
      res.status(fallback.statusCode);
      res.send(fallback.json);
      next();
      return;
    }

    if (fallback.type === "REDIRECT") {
      createProxyMiddleware({ target: fallback.baseUrl, changeOrigin: true })(
        req,
        res,
        next
      );
      return;
    }

    res.status(404);
    res.send();
    next();
  });
}
